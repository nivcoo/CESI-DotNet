using MainApplication.Localization;
using MainApplication.ViewModels;
namespace ConsoleApplication.Views;

/// <summary>
/// Here is the view for the console application you'll also find the language selection
/// And all the interactions for the console application.
/// 
/// </summary>
public class EasySaveView
{
    private readonly EasySaveViewModel _easySaveViewModel;
    public EasySaveView()
    {
        _easySaveViewModel = new EasySaveViewModel();
        InitView();
    }

    private void InitView()
    {
        
        
        var success = false;
        while (!success)
        {
            Console.WriteLine("Please Select Language : en-US, fr-FR");
            _easySaveViewModel.LanguageString = Console.ReadLine();
            success = _easySaveViewModel.UpdateLanguage();
        }
        
        Console.WriteLine(Language.GLOBAL_SELECTED_LANGUAGE);

        
        
        Console.WriteLine(_easySaveViewModel.Output);
    }
}