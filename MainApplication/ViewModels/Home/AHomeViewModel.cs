using System.Collections.ObjectModel;
using System.Globalization;
using MainApplication.Handlers;
using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;

namespace MainApplication.ViewModels.Home;

public abstract class AHomeViewModel : BaseViewModel
{
    private CommandHandler? _removeEncryptExtensionButtonEvent;

    private CommandHandler? _removePriorityFileButtonEvent;

    private FileType[] _fileTypes = (FileType[]) Enum.GetValues(typeof(FileType));

    public ObservableCollection<string> EncryptExtensions { get; }

    public ObservableCollection<string> PriorityFiles { get; }

    public AHomeViewModel()
    {
        EncryptExtensions = new ObservableCollection<string>();

        PriorityFiles = new ObservableCollection<string>();

        UpdateEncryptExtensionsList();

        UpdatePriorityFilesList();

        UpdateStats();
    }

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

    public abstract List<CultureInfo> AllCultureInfo { get; set; }

    public abstract CultureInfo SelectedCultureInfo { get; set; }

    public FileType[] FileTypes
    {
        get => _fileTypes;
        set => SetField(ref _fileTypes, value, nameof(FileTypes));
    }

    public abstract double MaxFileSize { get; set; }

    public abstract FileType SelectedSavesFileType { get; set; }

    public abstract FileType SelectedLogsFileType { get; set; }


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

    public abstract void UpdateStats();

    public abstract void RemoveEncryptExtensionEvent(object? args);

    public abstract void RemovePriorityFileEvent(object? args);


    public abstract bool StartSave(string name);

    public abstract void StartAllSaves();

    public abstract bool RemoveSave(string saveName);

    public abstract bool IsRunningSave(string? saveName);

    public abstract void UpdateEncryptExtensionsList();

    public abstract void UpdatePriorityFilesList();


    public abstract double GetProgressionOfAllSave();

    public abstract Tuple<int, int> GetFilesInformationsOfSave(Save save);

    public abstract Tuple<int, int> GetFilesInformationsOfAllSave();

    public static double GetProgressionOfSave(Save save)
    {
        return save.Progression;
    }

    public static bool IsCorrectLanguage(string language)
    {
        return LanguageCheck.CorrectLanguage(language);
    }
}