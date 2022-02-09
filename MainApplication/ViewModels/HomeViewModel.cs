
using System.Globalization;
using MainApplication.Localization;
using MainApplication.Objects;
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

    public bool UpdateLanguage()
    {
        Language.Culture = SaveService.SelectedCultureInfo;
        return true;
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

    public double GetProgressionOfSave(Save save)
    {
        return SaveService.GetProgressionOfSave(save);
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
}