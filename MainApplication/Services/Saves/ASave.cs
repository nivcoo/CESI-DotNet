using MainApplication.Objects;
using MainApplication.Objects.Enums;

namespace MainApplication.Services.Saves;

public abstract class ASave
{
    private readonly EasySaveService _easySaveServiceService = EasySaveService.GetInstance();
    private readonly LogService _logService = LogService.GetInstance();
    private readonly SaveService _saveService = SaveService.GetInstance();

    private readonly Mutex BigFilesMutex;

    protected bool DeleteFilesBeforeCopy = false;
    public bool PausedTask;

    public bool Running;

    protected List<SaveFile> SaveFiles;

    public Task<bool>? SaveTask;

    protected ASave(Save save)
    {
        BigFilesMutex = new Mutex();
        Init();
        SaveFiles = new List<SaveFile>();
        Save = save;
    }


    public CancellationTokenSource? TaskTokenSource { get; set; }

    public CancellationToken TaskToken { get; set; }

    protected Save Save { get; }

    public void Init()
    {
        InitTask();
        SaveFiles = new List<SaveFile>();
    }

    public void CancelTask()
    {
        try
        {
            if (TaskTokenSource != null)
                TaskTokenSource.Cancel();
        }
        catch (ObjectDisposedException)
        {
        }
    }

    private void InitTask()
    {
        Running = false;
        PausedTask = false;
        CancelTask();

        TaskTokenSource = new CancellationTokenSource();
        TaskToken = TaskTokenSource.Token;
        SaveTask = new Task<bool>(RunSave, TaskToken);
    }

    /// <summary>
    ///     Get all files in folder recursively
    /// </summary>
    /// <param name="path"></param>
    /// <returns>List of files</returns>
    protected static string[] GetAllFolderFiles(Uri path)
    {
        return Directory.Exists(path.LocalPath)
            ? Directory.GetFiles(path.LocalPath, "*.*", SearchOption.AllDirectories)
            : Array.Empty<string>();
    }

    /// <summary>
    ///     Delete not empty folder
    /// </summary>
    /// <param name="folderPath"></param>
    private static void DeleteFolderWithFiles(Uri folderPath)
    {
        if (!Directory.Exists(folderPath.LocalPath))
            return;

        var filePaths = GetAllFolderFiles(folderPath);
        try
        {
            foreach (var filePath in filePaths)
                File.Delete(filePath);

            Directory.Delete(folderPath.LocalPath, true);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    /// <summary>
    ///     Run current save
    /// </summary>
    /// <returns>true if Success</returns>
    private bool RunSave()
    {
        Running = true;
        ResetSaveValues();
        ChangeSaveState(State.Active);
        if (!RetrieveFilesToCopy())
        {
            EndSave();
            return false;
        }

        OrderFilesToCopy();
        UpdateStartSaveStatut();
        if (!CopyFiles())
        {
            EndSave();
            return false;
        }

        EndSave();
        return true;
    }

    private void EndSave()
    {
        ChangeSaveState(State.End);
        InitTask();
    }

    /// <summary>
    ///     Reset all default values
    /// </summary>
    private void ResetSaveValues()
    {
        SaveFiles = new List<SaveFile>();
        ExecuteActionOnUIThread(() => { Save.ResetValues(); });
    }

    /// <summary>
    ///     Update save into storage
    /// </summary>
    private void UpdateSaveStatut()
    {
        ExecuteActionOnUIThread(() =>
        {
            Save.NbFilesLeftToDo -= 1;
            Save.FilesAlreadyDone += 1;
            Save.UpdateProgression();
        });
        UpdateSaveStorage();
    }

    /// <summary>
    ///     Update save default values into storage
    /// </summary>
    private void UpdateStartSaveStatut()
    {
        ExecuteActionOnUIThread(() =>
        {
            Save.TotalFilesToCopy = SaveFiles.Count;
            Save.NbFilesLeftToDo = SaveFiles.Count;
            Save.TotalFilesSize = SaveFiles.Sum(saveFile => saveFile.FileSize);
        });
        UpdateSaveStorage();
    }

    /// <summary>
    ///     Change state of save (End, Active)
    /// </summary>
    /// <param name="state"></param>
    private void ChangeSaveState(State state)
    {
        ExecuteActionOnUIThread(() => { Save.State = state; });
        UpdateSaveStorage();
    }

    /// <summary>
    ///     Update save into storage
    /// </summary>
    private void UpdateSaveStorage()
    {
        _saveService.UpdateSaveStorage(Save);
    }

    protected abstract bool RetrieveFilesToCopy();

    private void OrderFilesToCopy()
    {
        var priorityFilesList = new List<SaveFile>();

        var otherFilesList = new List<SaveFile>();

        foreach (var file in SaveFiles)
            if (ConfigurationService.GetInstance().Config.PriorityFiles.Contains(file.FileName))
                priorityFilesList.Add(file);
            else
                otherFilesList.Add(file);

        SaveFiles = priorityFilesList.Concat(otherFilesList).ToList();
    }

    /// <summary>
    ///     Copy all retrieved files
    /// </summary>
    /// <returns>true if Success</returns>
    private bool CopyFiles()
    {
        if (SaveFiles.Count <= 0)
            return false;
        var sourceLocalPath = Save.SourcePath.LocalPath;
        if (!Directory.Exists(sourceLocalPath))
            return false;
        var targetLocalPath = Save.TargetPath.LocalPath;
        if (DeleteFilesBeforeCopy)
            DeleteFolderWithFiles(Save.TargetPath);
        foreach (var saveFile in SaveFiles)
        {
            var actualTimestamp = ToolService.GetTimestamp();
            var sourceFolder = saveFile.Path;
            var localPath = sourceFolder.Replace(sourceLocalPath, "");
            var targetFolder = targetLocalPath + localPath;
            Directory.CreateDirectory(targetFolder);
            var fileName = saveFile.FileName;
            var sourceFilePath = Path.Combine(sourceFolder, fileName);
            var targetFilePath = Path.Combine(targetFolder, fileName);


            var megaBytes = (double) saveFile.FileSize / 1000000;
            var isBigFile = megaBytes >= 10;
            if (isBigFile)
                BigFilesMutex.WaitOne(300000);
            long timeToEncypt = -1;
            try
            {
                if (ConfigurationService.GetInstance().Config.EncryptExtensions
                    .Contains(Path.GetExtension(saveFile.FileName)))
                    timeToEncypt = CryptoSoft.CryptoSoft.EncryptFile(sourceFilePath, targetFilePath,
                        "IO7LHYO8");
                else
                    File.Copy(sourceFilePath, targetFilePath, true);
            }
            catch (Exception)
            {
                // ignored
            }

            if (isBigFile)
                BigFilesMutex.ReleaseMutex();
            while (PausedTask && !IsCancelled())
            {
            }

            if (IsCancelled())
            {
                TaskTokenSource?.Dispose();
                return false;
            }

            UpdateSaveStatut();
            var sourceFileInfo = new FileInfo(sourceFilePath);
            var finalTimestamp = ToolService.GetTimestamp();
            var time = finalTimestamp - actualTimestamp;


            _logService.InsertLog(new Log(Save.Name, new Uri(sourceFilePath), new Uri(targetFilePath),
                sourceFileInfo.Length, time, timeToEncypt, DateTime.Now));
        }

        return true;
    }

    public bool IsCancelled()
    {
        return TaskTokenSource is {IsCancellationRequested: true};
    }

    public void ExecuteActionOnUIThread(Action action)
    {
        var uiThread = _easySaveServiceService.DispatchUiAction;

        if (uiThread == null)
            action.Invoke();
        else
            uiThread.Invoke(action.Invoke);
    }
}