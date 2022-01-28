using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services.Saves;
using MainApplication.Storages;

namespace MainApplication.Services;

internal sealed class SaveService
{
    private static readonly SaveService Instance = new();

    private readonly string _savesPath;

    private readonly AStorage<Save> _storage;

    private readonly List<Save> _saves;

    private readonly IDictionary<Save, ASave> _saveTasks;

    private SaveService()
    {
        _saveTasks = new Dictionary<Save, ASave>();
        _savesPath = @"data\saves.json";
        _storage = new JsonStorage<Save>(_savesPath);
        LoadSavesFile();
        _saves = new List<Save>();
        InitSavesList();
    }

    private void LoadSavesFile()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_savesPath) ?? string.Empty);
        if (!File.Exists(_savesPath))
            File.CreateText(_savesPath).Close();
    }

    private void InitSavesList()
    {
        foreach (var save in _storage.GetAllElements()) AddSaveToList(save);
    }

    public List<Save> GetSaves()
    {
        return _saves;
    }

    public bool StartSave(string saveName)
    {
        var save = AlreadySaveWithSameName(saveName);
        return save != null && StartSave(save);
    }

    private bool StartSave(Save save)
    {
        if (IsRunningSave(save))
            return false;
        ASave aSave;
        if (_saveTasks.ContainsKey(save))
            aSave = _saveTasks.First(x => x.Key == save).Value;
        else
        {
            aSave = save.Type switch
            {
                TypeSave.Complete => new CompleteASave(save),
                TypeSave.Differential => new DifferentialSave(save),
                _ => new CompleteASave(save)
            };
            _saveTasks.Add(save, aSave);
        }

        if (aSave.SaveTask.Status != TaskStatus.Running)
            aSave.SaveTask.Start();
        else
            return false;
        return true;
    }


    public bool IsRunningSave(string? saveName)
    {
        var save = AlreadySaveWithSameName(saveName);
        return save != null && IsRunningSave(save);
    }

    private bool IsRunningSave(Save save)
    {
        if (!_saveTasks.ContainsKey(save))
            return false;
        var aSave = _saveTasks.First(x => x.Key == save).Value;
        return aSave.SaveTask.Status == TaskStatus.Running;
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
        if (_saves.Count >= 5 || AlreadySaveWithSameName(save.Name) != null || save.Name == "all")
            return false;
        _storage.AddNewElement(save);
        AddSaveToList(save);
        return true;
    }

    private void AddSaveToList(Save save)
    {
        _saves.Add(save);
    }

    private bool RemoveSave(Save save)
    {
        if (IsRunningSave(save))
            return false;
        if (!_saves.Contains(save))
            return false;
        _storage.RemoveElement(s => s.Name == save.Name);
        _saves.Remove(save);
        return true;
    }

    public bool RemoveSave(string? saveName)
    {
        var save = AlreadySaveWithSameName(saveName);
        return save != null && RemoveSave(save);
    }

    public Save? AlreadySaveWithSameName(string? name)
    {
        return _saves.Find(save => save.Name == name);
    }

    public static SaveService GetInstance()
    {
        return Instance;
    }

    public void UpdateSaveStorage(Save save)
    {
        _storage.EditElementBy(s => s.Name == save.Name, save);
    }
    

    public static double GetProgressionOfSave(Save save)
    {
        return save.Progression;
    }

    public double GetProgressionOfAllSave()
    {
        return _saves.Sum(save => save.Progression) / _saves.Count;
    }
    

    public static Tuple<int, int> GetFilesInformationsOfSave(Save save)
    {
        return new Tuple<int, int>(save.NbFilesLeftToDo, save.TotalFilesToCopy);
    }

    public Tuple<int, int> GetFilesInformationsOfAllSave()
    {
        return new Tuple<int, int>(_saves.Sum(save => save.NbFilesLeftToDo), _saves.Sum(save => save.TotalFilesToCopy));
    }
}