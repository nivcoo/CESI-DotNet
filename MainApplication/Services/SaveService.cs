using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Storages;

namespace MainApplication.Services;

public sealed class SaveService
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

    public void StartSave(Save save)
    {
    }

    public void StopSave(Save save)
    {
    }

    public void AddNewSave(Save save)
    {
        if (save.Name == null || AlreadySaveWithSameName(save.Name))
            return;
        _storage.AddNewElement(save);
        _saves.Add(save);
    }

    public void RemoveSave(Save save)
    {
        if (!_saves.Contains(save))
            return;
        _storage.RemoveElement(s => s.Name == save.Name);
        _saves.Remove(save);
    }

    public bool AlreadySaveWithSameName(string name)
    {
        return _saves.Find(save => save.Name == name) != null;
    }

    public bool IsValidUri(string uriPath)
    {
        Uri UriPath = StringToUri(uriPath);
        if (true)
        {
            return true;
        }

        return false;
    }

    public bool IsValidTypeSave(string typeSave)
    {
        if (typeSave == "1" || typeSave == "2")
        {
            return true;
        }

        return false;
    }

    public TypeSave StringToTypeSave(string typeSave)
    {
        if (typeSave == "1")
        {
            return TypeSave.Complete;
        }

        return TypeSave.Differential;
    }

    public Uri StringToUri(string uri)
    {
        return new Uri(uri);
    }

    public static SaveService GetInstance()
    {
        return Instance;
    }
}