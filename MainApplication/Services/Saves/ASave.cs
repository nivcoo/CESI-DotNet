using System.Security.Cryptography;
using MainApplication.Objects;
using MainApplication.Objects.Enums;

namespace MainApplication.Services.Saves;

public abstract class ASave
{
    internal readonly LogService LogService = LogService.GetInstance();
    private readonly SaveService _saveService = SaveService.GetInstance();

    public readonly Task<bool> SaveTask;
    protected Save Save { get; set; }
    private readonly SHA256 _sha256 = SHA256.Create();

    protected readonly List<SaveFile> SaveFiles;

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

    protected byte[] GetHashSha256(string fileName)
    {
        using var stream = File.OpenRead(fileName);
        return _sha256.ComputeHash(stream);
    }

    protected static void DeleteFolderWithFiles(Uri folderPath)
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

    protected void UpdateSaveStatut()
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

    protected abstract bool CopyFiles();
}