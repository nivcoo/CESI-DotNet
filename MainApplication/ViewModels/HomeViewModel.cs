using System.Collections.ObjectModel;
using System.Globalization;
using MainApplication.Handlers;
using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services;

namespace MainApplication.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private CommandHandler? _removeEncryptExtensionButtonEvent;

    private CommandHandler? _removePriorityFileButtonEvent;

    private FileType[] _fileTypes = (FileType[]) Enum.GetValues(typeof(FileType));

    public HomeViewModel()
    {
        EncryptExtensions = new ObservableCollection<string>();

        PriorityFiles = new ObservableCollection<string>();

        UpdateEncryptExtensionsList();

        UpdatePriorityFilesList();

        UpdateStats();
    }

    public ObservableCollection<string> EncryptExtensions { get; }

    public ObservableCollection<string> PriorityFiles { get; }

    public CommandHandler RemoveEncryptExtensionButtonEvent
    {
        get
        {
            return _removeEncryptExtensionButtonEvent ??=
                _removeEncryptExtensionButtonEvent = new CommandHandler(RemoveEncryptExtensionEvent);
        }
    }

    public CommandHandler RemovePriorityFileButtonEvent
    {
        get
        {
            return _removePriorityFileButtonEvent ??=
                _removePriorityFileButtonEvent = new CommandHandler(RemovePriorityFileEvent);
        }
    }

    public List<CultureInfo> AllCultureInfo
    {
        get => EasySaveService.AllCultureInfo;
        set => SetField(ref EasySaveService.AllCultureInfo, value, nameof(AllCultureInfo));
    }

    public CultureInfo SelectedCultureInfo
    {
        get => EasySaveService.SelectedCultureInfo;
        set => EasySaveService.ChangeCulture(value);
    }

    public FileType[] FileTypes
    {
        get => _fileTypes;
        set => SetField(ref _fileTypes, value, nameof(FileTypes));
    }

    public double MaxFileSize
    {
        get => ConfigurationService.Config.MaxFileSize;
        set => ConfigurationService.ChangeMaxFileSize(value);
    }


    public FileType SelectedSavesFileType
    {
        get => ConfigurationService.Config.SavesFileType;
        set => ConfigurationService.ChangeSavesFileType(value);
    }

    public FileType SelectedLogsFileType
    {
        get => ConfigurationService.Config.LogsFileType;
        set => ConfigurationService.ChangeLogsFileType(value);
    }


    private int _statSavesNumber;
    public int StatSavesNumber
    {
        get => _statSavesNumber;
        set => SetField(ref _statSavesNumber, value, nameof(StatSavesNumber));
    }

    private int _statLogsNumber;
    public int StatLogsNumber
    {
        get => _statLogsNumber;
        set => SetField(ref _statLogsNumber, value, nameof(StatLogsNumber));
    }

    public void UpdateStats()
    {
        StatSavesNumber = SaveService.GetSaves().Count;
        StatLogsNumber = LogService.GetAllLogFiles().Count;
    }

    private void RemoveEncryptExtensionEvent(object? args)
    {
        if (args is not string extension)
            return;

        ConfigurationService.RemoveEncryptExtension(extension);
        EncryptExtensions.Remove(extension);
    }

    private void RemovePriorityFileEvent(object? args)
    {
        if (args is not string priorityFile)
            return;

        ConfigurationService.RemovePriorityFile(priorityFile);
        PriorityFiles.Remove(priorityFile);
    }


    public bool StartSave(string name)
    {
        return SaveService.StartSave(name);
    }

    public void StartAllSaves()
    {
        SaveService.StartAllSaves();
    }

    public bool RemoveSave(string saveName)
    {
        return SaveService.RemoveSave(saveName);
    }

    public bool IsRunningSave(string? saveName)
    {
        return SaveService.IsRunningSave(saveName);
    }

    public static double GetProgressionOfSave(Save save)
    {
        return save.Progression;
    }

    public void UpdateEncryptExtensionsList()
    {
        EncryptExtensions.Clear();
        foreach (var extensionName in ConfigurationService.Config.EncryptExtensions)
            EncryptExtensions.Add(extensionName);
    }

    public void UpdatePriorityFilesList()
    {
        PriorityFiles.Clear();
        foreach (var fileName in ConfigurationService.Config.PriorityFiles)
            PriorityFiles.Add(fileName);
    }


    public double GetProgressionOfAllSave()
    {
        return SaveService.GetProgressionOfAllSave();
    }

    public Tuple<int, int> GetFilesInformationsOfSave(Save save)
    {
        return SaveService.GetFilesInformationsOfSave(save);
    }

    public Tuple<int, int> GetFilesInformationsOfAllSave()
    {
        return SaveService.GetFilesInformationsOfAllSave();
    }

    public static bool IsCorrectLanguage(string language)
    {
        return LanguageCheck.CorrectLanguage(language);
    }
}