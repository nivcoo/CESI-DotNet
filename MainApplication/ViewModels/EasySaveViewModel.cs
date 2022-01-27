using System.Globalization;
using System.Windows.Input;
using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;

namespace MainApplication.ViewModels;

public class EasySaveViewModel : BaseViewModel
{
    private ICommand? _clickCommand;
    // public ICommand ClickCommand
    // {
    //     get { return _clickCommand ??= _clickCommand = new CommandHandler(() => ConvertToInt()); }
    // }


    private string? _languageString;

    public string? LanguageString
    {
        get => _languageString;
        set => SetField(ref _languageString, value, nameof(LanguageString));
    }


    private List<Save>? _saves;

    public List<Save>? Saves
    {
        get => _saves;
        set => SetField(ref _saves, value, nameof(Saves));
    }

    public void UpdateSavesList()
    {
        _saves = SaveService.GetSaves();
    }

    public bool UpdateLanguage()
    {
        /*IStorage isStorage = new JsonStorage("saves.json");

        isStorage.AddNewElement(new Save
        {
            Name = "trt9",
            Type = TypeSave.Complete
        });

        Console.WriteLine(isStorage.GetAllElements<Save>()[2].Name);

        IStorage<Log> isStorage = new JsonStorage<Log>(@"datas\logs.json");
        isStorage.ClearFile();
        isStorage.AddNewElementWithoutRewrite(new Log
        {
            Name = "IOUIVUGYG",
            SourcePath = new Uri(Directory.GetCurrentDirectory() + @"\datas\logs.json"),
            Date = DateTime.Now
        });
        isStorage.AddNewElementWithoutRewrite(new Log
        {
            Name = "IOUIVUGYG222",
            Date = DateTime.Now
        });
        Console.WriteLine(isStorage.GetAllElements()[0].SourcePath);

        */
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
}