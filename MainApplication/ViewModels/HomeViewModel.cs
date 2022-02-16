
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

    public ObservableCollection<string> EncryptExtensions { get; }

    public ObservableCollection<string> PriorityFiles { get; }

    private CommandHandler? _removeEncryptExtensionButtonEvent;

    public CommandHandler RemoveEncryptExtensionButtonEvent
    {
        get { return _removeEncryptExtensionButtonEvent ??= _removeEncryptExtensionButtonEvent = new CommandHandler(RemoveEncryptExtensionEvent); }
    }

    private CommandHandler? _removePriorityFileButtonEvent;

    public CommandHandler RemovePriorityFileButtonEvent
    {
        get { return _removePriorityFileButtonEvent ??= _removePriorityFileButtonEvent = new CommandHandler(RemovePriorityFileEvent); }
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

    private SaveFileType[] _saveFileTypes = (SaveFileType[])Enum.GetValues(typeof(SaveFileType));

    public SaveFileType[] SaveFileTypes
    {
        get => _saveFileTypes;
        set => SetField(ref _saveFileTypes, value, nameof(SaveFileTypes));
    }


    public SaveFileType SelectedSaveFileType
    {
        get => ConfigurationService.Config.SaveFileType;
        set => ConfigurationService.ChangeSaveFileType(value);
    }

    public HomeViewModel() {
        EncryptExtensions = new ObservableCollection<string>();

        PriorityFiles = new ObservableCollection<string>();

        UpdateEncryptExtensionsList();

        UpdatePriorityFilesList();
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