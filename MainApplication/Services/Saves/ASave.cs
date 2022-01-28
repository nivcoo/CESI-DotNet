using System.Security.Cryptography;
using MainApplication.Objects;
using MainApplication.Objects.Enums;

namespace MainApplication.Services.Saves;

public abstract class ASave
{
    private readonly LogService _logService = LogService.GetInstance();
    private readonly SaveService _saveService = SaveService.GetInstance();

    public readonly Task<bool> SaveTask;
    protected Save Save { get; set; }
    private readonly SHA256 _sha256 = SHA256.Create();

    protected readonly List<SaveFile> SaveFiles;

    protected bool DeleteFilesBeforeCopy = false;

    protected ASave(Save save)
    {
        SaveTask = new Task<bool>(RunSave);
        SaveFiles = new List<SaveFile>();
        Save = save;
    }

    protected static string[] GetAllFolderFiles(Uri path)
    {
        return Directory.GetFiles(path.LocalPath, "*.*", SearchOption.AllDirectories);
    }

    private static void DeleteFolderWithFiles(Uri folderPath)
    {
        var filePaths = GetAllFolderFiles(folderPath);
        foreach (var filePath in filePaths)
            File.Delete(filePath);

        Directory.Delete(folderPath.LocalPath, true);
    }

    private bool RunSave()
    {
        ResetSaveValues();
        ChangeSaveState(State.Active);
        if (!RetrieveFilesToCopy())
        {
            ChangeSaveState(State.End);
            return false;
        }

        UpdateStartSaveStatut();
        if (!CopyFiles())
        {
            ChangeSaveState(State.End);
            return false;
        }

        ChangeSaveState(State.End);

        return true;
    }

    private void ResetSaveValues()
    {
        Save.NbFilesLeftToDo = 0;
        Save.TotalFilesToCopy = 0;
        Save.Progression = 0;
    }

    private void UpdateSaveStatut()
    {
        Save.NbFilesLeftToDo -= 1;
        Save.UpdateProgression();
        UpdateSaveStorage();
    }

    private void UpdateStartSaveStatut()
    {
        Save.TotalFilesToCopy = SaveFiles.Count;
        Save.NbFilesLeftToDo = SaveFiles.Count;
        Save.TotalFilesSize = SaveFiles.Sum(saveFile => saveFile.FileSize);
        UpdateSaveStorage();
    }

    private void ChangeSaveState(State state)
    {
        Save.State = state;
        UpdateSaveStorage();
    }

    private void UpdateSaveStorage()
    {
        _saveService.UpdateSaveStorage(Save);
    }

    protected abstract bool RetrieveFilesToCopy();

    private bool CopyFiles()
    {
        if (SaveFiles.Count <= 0)
            return false;
        var sourceLocalPath = Save.SourcePath.LocalPath;
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
            try
            {
                File.Copy(sourceFilePath, targetFilePath, true);
                UpdateSaveStatut();
                var sourceFileInfo = new FileInfo(sourceFilePath);
                var finalTimestamp = ToolService.GetTimestamp();
                var time = finalTimestamp - actualTimestamp;
                _logService.InsertLog(new Log(Save.Name, new Uri(sourceFilePath), new Uri(targetFilePath),
                    sourceFileInfo.Length, time, DateTime.Now));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return true;
    }
}