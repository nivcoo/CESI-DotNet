using System.Security.Cryptography;
using MainApplication.Objects;
using MainApplication.Objects.Enums;

namespace MainApplication.Services.Saves;

public abstract class ASave
{
    internal readonly LogService LogService = LogService.GetInstance();
    private readonly SaveService _saveService = SaveService.GetInstance();
    protected Save Save { get; set; }
    private readonly SHA256 _sha256 = SHA256.Create();

    protected readonly List<SaveFile> SaveFiles;

    protected ASave(Save save)
    {
        SaveFiles = new List<SaveFile>();
        Save = save;
    }

    public static string[] GetAllFolderFiles(Uri path)
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

    public bool RunSave()
    {
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

        ChangeSaveState(State.Active);
        return true;
    }

    protected void UpdateSaveStatut()
    {
        Save.NbFilesLeftToDo -= 1;
        Save.UpdateProgression();
        UpdateSaveStorage();
    }

    private void ChangeSaveState(State state)
    {
        Save.State = state;
        UpdateSaveStorage();
    }

    protected void UpdateSaveStorage()
    {
        _saveService.UpdateSaveStorage(Save);
    }

    protected abstract bool RetrieveFilesToCopy();

    protected abstract void UpdateStartSaveStatut();

    protected abstract bool CopyFiles();
}