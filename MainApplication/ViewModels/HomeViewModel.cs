
using System.Globalization;
using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services;

namespace MainApplication.ViewModels;

public class HomeViewModel : BaseViewModel
{



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