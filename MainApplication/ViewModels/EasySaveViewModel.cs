using System.Globalization;
using System.Windows.Input;
using MainApplication.Handlers;
using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Storages;

namespace MainApplication.ViewModels;

public class EasySaveViewModel : BaseViewModel
{
    private ICommand? _clickCommand;

    public ICommand ClickCommand
    {
        get { return _clickCommand ??= _clickCommand = new CommandHandler(() => ConvertToInt()); }
    }


    private string? _input;

    public string? Input
    {
        get => _input;
        set => SetField(ref _input, value, nameof(Input));
    }

    private int _output;

    public int Output
    {
        get => _output;
        set => SetField(ref _output, value, nameof(Output));
    }

    private string? _languageString;

    public string? LanguageString
    {
        get => _languageString;
        set => SetField(ref _languageString, value, nameof(LanguageString));
    }

    public bool UpdateLanguage()
    {
        /*IStorage isStorage = new JsonStorage("saves.json");

        isStorage.AddNewElement(new Save
        {
            Name = "trt9",
            Type = TypeSave.Complete
        });

        Console.WriteLine(isStorage.GetAllElements<Save>()[2].Name);*/

        if (LanguageString == null)
            return false;
        if (!LanguageCheck.CorrectLanguage(LanguageString))
            return false;
        Language.Culture = CultureInfo.GetCultureInfo(LanguageString);
        return true;
    }

    public bool ConvertToInt()
    {
        try
        {
            var number = Convert.ToInt32(Input);
            int[] correctNumber = {1, 2, 3};
            var pos = Array.IndexOf(correctNumber, number);
            if (pos <= -1)
                return true;
            Output = number;
        }
        catch (FormatException)
        {
            return true;
        }

        return false;
    }
}