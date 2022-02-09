using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services.Saves;
using MainApplication.Storages;
using System.Globalization;

namespace MainApplication.Services;

internal sealed class SaveService
{
    private static readonly SaveService Instance = new();

    private readonly string _savesPath;

    private readonly AStorage<Save> _storage;

    private readonly List<Save> _saves;

    private readonly IDictionary<Save, ASave> _saveTasks;

    public CultureInfo SelectedCultureInfo;

    private SaveService()
    {
        SelectedCultureInfo = CultureInfo.CurrentCulture;
        _saveTasks = new Dictionary<Save, ASave>();
        _savesPath = AppDomain.CurrentDomain.BaseDirectory + @"data\saves.json";
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
        foreach (var save in _storage.GetAllElements()) {

            save.ResetValues();
            AddSaveToList(save);
        }
    }
    
    /// <summary>
    /// Get all saves
    /// </summary>
    /// <returns>Save List</returns>
    public List<Save> GetSaves()
    {
        return _saves;
    }

    /// <summary>
    /// Start save with name
    /// </summary>
    /// <param name="saveName"></param>
    /// <returns>Success</returns>
    public bool StartSave(string saveName)
    {
        var save = AlreadySaveWithSameName(saveName);
        return save != null && StartSave(save);
    }

    /// <summary>
    /// Start save with save object
    /// </summary>
    /// <param name="save"></param>
    /// <returns>true if Success</returns>
    public bool StartSave(Save save)
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

        aSave.CancelTask();

        while (aSave.Running) { } // wait stop correctly

        aSave.Init();

        if (aSave.SaveTask.Status == TaskStatus.Created)
            aSave.SaveTask.Start();
        else return false;
        return true;
    }

    public void SetStateOfSave(Save save, bool paused) {
        if (!_saveTasks.ContainsKey(save) || save.State == State.End)
            return;
        ASave aSave = _saveTasks.First(x => x.Key == save).Value;
        if (paused)
            save.State = State.Pause;
        else
            save.State = State.Active;
        aSave.PausedTask = paused;
    }


    /// <summary>
    /// Check if save is running
    /// </summary>
    /// <param name="saveName"></param>
    /// <returns>true if Running</returns>
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
        return aSave.SaveTask.Status == TaskStatus.Running && aSave.PausedTask == false;
    }

    /// <summary>
    /// Start all saves
    /// </summary>
    public void StartAllSaves()
    {
        foreach (var save in _saves)
        {
            StartSave(save);
        }
    }

    public void PauseAllSaves()
    {
        foreach (var save in _saves)
        {
            SetStateOfSave(save, true);
        }
    }

    public void ResumeAllSaves()
    {
        foreach (var save in _saves)
        {
            SetStateOfSave(save, false);
        }
    }


    /// <summary>
    /// Add new save with save object
    /// </summary>
    /// <param name="save"></param>
    /// <returns>tru if Success</returns>
    public bool AddNewSave(Save save)
    {
        if (AlreadySaveWithSameName(save.Name) != null || save.Name == "all")
            return false;
        _storage.AddNewElement(save);
        AddSaveToList(save);
        return true;
    }

    private void AddSaveToList(Save save)
    {
        save.State = State.End;
        _saves.Add(save);
    }
    
    /// <summary>
    /// Remove save with save object
    /// </summary>
    /// <param name="save"></param>
    /// <returns>true if Success</returns>
    public bool RemoveSave(Save save)
    {
        if (IsRunningSave(save))
            return false;
        if (!_saves.Contains(save))
            return false;
        _storage.RemoveElement(s => s.Name == save.Name);
        _saves.Remove(save);
        return true;
    }
    
    
    /// <summary>
    /// Remove save with name
    /// </summary>
    /// <param name="saveName"></param>
    /// <returns>true if Success</returns>
    public bool RemoveSave(string? saveName)
    {
        var save = AlreadySaveWithSameName(saveName);
        return save != null && RemoveSave(save);
    }

    /// <summary>
    /// Check if save exist with same name
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Save object</returns>
    public Save? AlreadySaveWithSameName(string? name)
    {
        return _saves.Find(save => save.Name == name);
    }

    public static SaveService GetInstance()
    {
        return Instance;
    }

    /// <summary>
    /// Update storage of save object
    /// </summary>
    /// <param name="save"></param>
    public void UpdateSaveStorage(Save save)
    {
        _storage.EditElementBy(s => s.Name == save.Name, save);
    }
    

    /// <summary>
    /// Get progression of specific save
    /// </summary>
    /// <param name="save"></param>
    /// <returns>The save progression</returns>
    public static double GetProgressionOfSave(Save save)
    {
        return save.Progression;
    }
    
    /// <summary>
    /// Get sum of all progression 
    /// </summary>
    /// <returns>The sum of all progression</returns>
    public double GetProgressionOfAllSave()
    {
        return _saves.Sum(save => save.Progression) / _saves.Count;
    }
    
    /// <summary>
    /// Get Number of file in specific save
    /// </summary>
    /// <returns>Number left to do, Total Number</returns>
    public static Tuple<int, int> GetFilesInformationsOfSave(Save save)
    {
        return new Tuple<int, int>(save.NbFilesLeftToDo, save.TotalFilesToCopy);
    }
    
    /// <summary>
    /// Get Number of file in all save (Sum)
    /// </summary>
    /// <returns>Number left to do, Total Number</returns>
    public Tuple<int, int> GetFilesInformationsOfAllSave()
    {
        return new Tuple<int, int>(_saves.Sum(save => save.NbFilesLeftToDo), _saves.Sum(save => save.TotalFilesToCopy));
    }
}