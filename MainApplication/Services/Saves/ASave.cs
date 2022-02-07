using MainApplication.Objects;
using MainApplication.Objects.Enums;

namespace MainApplication.Services.Saves;

public abstract class ASave
{
    private readonly LogService _logService = LogService.GetInstance();
    private readonly SaveService _saveService = SaveService.GetInstance();

    public Task<bool> SaveTask;
    protected Save Save { get; }

    protected readonly List<SaveFile> SaveFiles;

    protected bool DeleteFilesBeforeCopy = false;

    protected ASave(Save save)
    {
        SaveTask = new Task<bool>(RunSave);
        SaveFiles = new List<SaveFile>();
        Save = save;
    }
    
    /// <summary>
    /// Get all files in folder recursively
    /// </summary>
    /// <param name="path"></param>
    /// <returns>List of files</returns>
    protected static string[] GetAllFolderFiles(Uri path)
    {
        if (Directory.Exists(path.LocalPath))
            return Directory.GetFiles(path.LocalPath, "*.*", SearchOption.AllDirectories);
        return Array.Empty<string>();
        
    }
    
    /// <summary>
    /// Delete not empty folder
    /// </summary>
    /// <param name="folderPath"></param>
    private static void DeleteFolderWithFiles(Uri folderPath)
    {
        var filePaths = GetAllFolderFiles(folderPath);
        foreach (var filePath in filePaths)
            File.Delete(filePath);

        if(Directory.Exists(folderPath.LocalPath))
            Directory.Delete(folderPath.LocalPath, true);
    }

    /// <summary>
    /// Run current save
    /// </summary>
    /// <returns>true if Success</returns>
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

        SaveTask = new Task<bool>(RunSave);
        return true;
    }
    
    /// <summary>
    /// Reset all default values
    /// </summary>
    private void ResetSaveValues()
    {
        Save.NbFilesLeftToDo = 0;
        Save.TotalFilesToCopy = 0;
        Save.Progression = 0;
    }

    /// <summary>
    /// Update save into storage
    /// </summary>
    private void UpdateSaveStatut()
    {
        Save.NbFilesLeftToDo -= 1;
        Save.UpdateProgression();
        UpdateSaveStorage();
    }

    /// <summary>
    /// Update save default values into storage
    /// </summary>
    private void UpdateStartSaveStatut()
    {
        Save.TotalFilesToCopy = SaveFiles.Count;
        Save.NbFilesLeftToDo = SaveFiles.Count;
        Save.TotalFilesSize = SaveFiles.Sum(saveFile => saveFile.FileSize);
        UpdateSaveStorage();
    }

    /// <summary>
    /// Change state of save (End, Active)
    /// </summary>
    /// <param name="state"></param>
    private void ChangeSaveState(State state)
    {

        UpdateSaveStorage();
    }

    /// <summary>
    /// Update save into storage
    /// </summary>
    private void UpdateSaveStorage()
    {
        _saveService.UpdateSaveStorage(Save);
    }
    
    protected abstract bool RetrieveFilesToCopy();

    /// <summary>
    /// Copy all retrieved files
    /// </summary>
    /// <returns>true if Success</returns>
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