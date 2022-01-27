using System.Security.Cryptography;
using MainApplication.Objects;
using MainApplication.Objects.Enums;

namespace MainApplication.Services.Saves;

public abstract class ASave
{
    internal readonly LogService LogService = LogService.GetInstance();
    internal readonly SaveService SaveService = SaveService.GetInstance();
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

    protected byte[] GetHashSha256(string filename)
    {
        using var stream = File.OpenRead(filename);
        return _sha256.ComputeHash(stream);
    }

    protected static void DeleteFolderWithFiles(Uri folderPath)
    {
        var filePaths = GetAllFolderFiles(folderPath);
        Console.WriteLine(filePaths);
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

    protected void ChangeSaveState(State state)
    {
        Save.State = state;
        UpdateSaveStorage();
    }

    public void UpdateSaveStorage()
    {
        SaveService.UpdateSaveStorage(Save);
    }

    public abstract bool RetrieveFilesToCopy();

    protected abstract void UpdateStartSaveStatut();

    public abstract bool CopyFiles();
}