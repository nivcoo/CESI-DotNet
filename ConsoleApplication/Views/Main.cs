using MainApplication.ViewModels;

namespace ConsoleApplication.Views;

public class Main
{
    private readonly MainViewModel _mainViewModel;
    public Main()
    {
        _mainViewModel = new MainViewModel();
        InitView();
    }

    private void InitView()
    {
        
        
        var success = false;
        while (!success)
        {
            Console.WriteLine("Please Select type : 1,2,3");
            _mainViewModel.Input = Console.ReadLine();
            success = !_mainViewModel.ConvertToInt();
        }

        
        
        Console.WriteLine(_mainViewModel.Output);
    }
}