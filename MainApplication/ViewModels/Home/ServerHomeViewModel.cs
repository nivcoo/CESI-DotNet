using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace MainApplication.ViewModels.Home;

public class ServerHomeViewModel : AHomeViewModel
{

    public ServerHomeViewModel() : base()
    {
        UpdateEncryptExtensionsList();

        UpdatePriorityFilesList();

        UpdateStats();

        ConfigurationService.Config.RegisterToEvent(NotifyPropertyChanged);
    }

    private void NotifyPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {

        switch(e.PropertyName)
        {
            case nameof(ConfigurationService.Config.MaxFileSize):
                OnPropertyChanged(nameof(MaxFileSize));
                UpdateGUI();
                break;
            case nameof(ConfigurationService.Config.SavesFileType):
                OnPropertyChanged(nameof(SelectedSavesFileType));
                UpdateGUI();
                break;

            case nameof(ConfigurationService.Config.LogsFileType):
                OnPropertyChanged(nameof(SelectedLogsFileType));

                UpdateGUI();
                break;

            case nameof(ConfigurationService.Config.Language):
                
                OnPropertyChanged(nameof(SelectedCultureInfo));
                OnPropertyChanged(nameof(AllCultureInfo));
                UpdateGUI();
                break;

        }
    }
    public void UpdateGUI()
    {
        UpdateLocalizationAction?.Invoke();
        UpdateComboBoxAction?.Invoke();
        UpdateStats();
        UpdateEncryptExtensionsList();
        UpdatePriorityFilesList();
    }
    

    public override double MaxFileSize
    {
        get => ConfigurationService.Config.MaxFileSize;
        set => ConfigurationService.ChangeMaxFileSize(value);
    }

    public override FileType SelectedSavesFileType
    {
        get => ConfigurationService.Config.SavesFileType;
        set => ConfigurationService.ChangeSavesFileType(value);
    }

    public override CultureInfo SelectedCultureInfo
    {
        get => EasySaveService.SelectedCultureInfo;
        set => EasySaveService.ChangeCulture(value);
    }

    public override List<CultureInfo> AllCultureInfo
    {
        get => EasySaveService.AllCultureInfo;
        set => SetField(ref EasySaveService.AllCultureInfo, value, nameof(AllCultureInfo));
    }

    public override FileType SelectedLogsFileType
    {
        get => ConfigurationService.Config.LogsFileType;
        set => ConfigurationService.ChangeLogsFileType(value);
    }

    public override void UpdateStats()
    {
        StatSavesNumber = SaveService.GetSaves().Count;
        StatLogsNumber = LogService.GetAllLogFiles().Count;
    }

    public override void RemoveEncryptExtensionEvent(object? args)
    {
        if (args is not string extension)
            return;

        ConfigurationService.RemoveEncryptExtension(extension);
        EncryptExtensions.Remove(extension);
    }


    public override void RemovePriorityFileEvent(object? args)
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

    public override void UpdateEncryptExtensionsList()
    {
        EncryptExtensions.Clear();
        foreach (var extensionName in ConfigurationService.Config.EncryptExtensions)
            EncryptExtensions.Add(extensionName);
    }

    public override void UpdatePriorityFilesList()
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
    public static double GetProgressionOfSave(Save save)
    {
        return save.Progression;
    }

    public static bool IsCorrectLanguage(string language)
    {
        return LanguageCheck.CorrectLanguage(language);
    }

}

