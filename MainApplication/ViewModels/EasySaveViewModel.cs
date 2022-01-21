using System.ComponentModel;
using System.Windows.Input;
using MainApplication.Handlers;

namespace MainApplication.ViewModels;

public class EasySaveViewModel : BaseViewModel
{
    
    private ICommand? _clickCommand;
    public ICommand ClickCommand
    {
        get
        {
            return _clickCommand ??= _clickCommand = new CommandHandler(() => ConvertToInt());
        }
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