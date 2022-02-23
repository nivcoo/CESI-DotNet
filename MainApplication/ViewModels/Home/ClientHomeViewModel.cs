using MainApplication.Objects;
using MainApplication.Objects.Enums;
using System.Globalization;

namespace MainApplication.ViewModels.Home;

public class ClientHomeViewModel : AHomeViewModel
{
    public override List<CultureInfo> AllCultureInfo 
    { 
        get => throw new NotImplementedException(); 
        set => throw new NotImplementedException(); 
    }
    public override CultureInfo SelectedCultureInfo 
    { 
        get => throw new NotImplementedException(); 
        set => throw new NotImplementedException(); 
    }
    public override double MaxFileSize 
    { 
        get => throw new NotImplementedException(); 
        set => throw new NotImplementedException(); 
    }
    public override FileType SelectedSavesFileType 
    { 
        get => throw new NotImplementedException(); 
        set => throw new NotImplementedException(); 
    }
    public override FileType SelectedLogsFileType 
    { 
        get => throw new NotImplementedException(); 
        set => throw new NotImplementedException(); 
    }

    public override Tuple<int, int> GetFilesInformationsOfAllSave()
    {
        throw new NotImplementedException();
    }

    public override Tuple<int, int> GetFilesInformationsOfSave(Save save)
    {
        throw new NotImplementedException();
    }

    public override double GetProgressionOfAllSave()
    {
        throw new NotImplementedException();
    }

    public override bool IsRunningSave(string? saveName)
    {
        throw new NotImplementedException();
    }

    public override void RemoveEncryptExtensionEvent(object? args)
    {
        throw new NotImplementedException();
    }

    public override void RemovePriorityFileEvent(object? args)
    {
        throw new NotImplementedException();
    }

    public override bool RemoveSave(string saveName)
    {
        throw new NotImplementedException();
    }

    public override void StartAllSaves()
    {
        throw new NotImplementedException();
    }

    public override bool StartSave(string name)
    {
        throw new NotImplementedException();
    }

    public override void UpdateEncryptExtensionsList()
    {
        throw new NotImplementedException();
    }

    public override void UpdatePriorityFilesList()
    {
        throw new NotImplementedException();
    }

    public override void UpdateStats()
    {
        throw new NotImplementedException();
    }
}

