using MainApplication.Objects;
using MainApplication.Objects.Enums;
using System.Globalization;

namespace MainApplication.ViewModels.Home;

public class ServerHomeViewModel : AHomeViewModel
{

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


    public override bool StartSave(string name)
    {
        return SaveService.StartSave(name);
    }

    public override void StartAllSaves()
    {
        SaveService.StartAllSaves();
    }

    public override bool RemoveSave(string saveName)
    {
        return SaveService.RemoveSave(saveName);
    }

    public override bool IsRunningSave(string? saveName)
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


    public override double GetProgressionOfAllSave()
    {
        return SaveService.GetProgressionOfAllSave();
    }

    public override Tuple<int, int> GetFilesInformationsOfSave(Save save)
    {
        return SaveService.GetFilesInformationsOfSave(save);
    }

    public override Tuple<int, int> GetFilesInformationsOfAllSave()
    {
        return SaveService.GetFilesInformationsOfAllSave();
    }

}

