using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services.Saves;
using MainApplication.Storages;

namespace MainApplication.Services;

internal sealed class SaveService
{
    private static readonly SaveService Instance = new();

    private readonly string _savesPath;

    private readonly IStorage<Save> _storage;

    private readonly List<Save> _saves;

    private SaveService()
    {
        _savesPath = @"data\saves.json";
        _storage = new JsonStorage<Save>(_savesPath);
        LoadSavesFile();


        _saves = _storage.GetAllElements();
    }

    private void LoadSavesFile()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_savesPath) ?? string.Empty);
        if (!File.Exists(_savesPath))
            File.CreateText(_savesPath).Close();
    }

    public List<Save> GetSaves()
    {
        return _saves;
    }
    
    public bool StartSave(string name)
    {
        var save = AlreadySaveWithSameName(name);
        return save != null && StartSave(save);
    }

    private static bool StartSave(Save save)
    {
        ASave aSave = save.Type switch
        {
            TypeSave.Complete => new CompleteASave(save),
            TypeSave.Differential => new DifferentialASave(save),
            _ => new CompleteASave(save)
        };

        return aSave.RunSave();
    }
    
    public void StartAllSaves()
    {
        foreach (var save in _saves)
        {
            StartSave(save);
        }
    }

    public void StopSave(Save save)
    {
    }

    public bool AddNewSave(Save save)
    {
        if (AlreadySaveWithSameName(save.Name) != null)
            return false;
        _storage.AddNewElement(save);
        _saves.Add(save);
        return true;
    }

    public bool RemoveSave(Save save)
    {
        if (!_saves.Contains(save))
            return false;
        _storage.RemoveElement(s => s.Name == save.Name);
        _saves.Remove(save);
        return true;
    }

    public Save? AlreadySaveWithSameName(string name)
    {
        return _saves.Find(save => save.Name == name);
    }

    public static SaveService GetInstance()
    {
        return Instance;
    }
}