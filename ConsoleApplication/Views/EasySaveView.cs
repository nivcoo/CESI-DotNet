using MainApplication.ViewModels;

namespace ConsoleApplication.Views;

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
            Console.WriteLine("Please Select type : 1,2,3");
            _easySaveViewModel.Input = Console.ReadLine();
            success = !_easySaveViewModel.ConvertToInt();
        }

        
        
        Console.WriteLine(_easySaveViewModel.Output);
    }
}