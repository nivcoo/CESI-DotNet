﻿using System.Globalization;
using System.Windows.Input;
using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services;

namespace MainApplication.ViewModels;

public class EasySaveViewModel : BaseViewModel
{


    private string? _languageString;

    public string? LanguageString
    {
        get => _languageString;
        set => SetField(ref _languageString, value, nameof(LanguageString));
    }

    public bool UpdateLanguage()
    {
        if (LanguageString == null)
            return false;
        if (!LanguageCheck.CorrectLanguage(LanguageString))
            return false;
        Language.Culture = CultureInfo.GetCultureInfo(LanguageString);
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