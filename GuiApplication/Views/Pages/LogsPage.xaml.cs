using MainApplication.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GuiApplication.Views.Pages;

public sealed partial class LogsPage : Page
{

    private readonly LogsViewModel _logsViewModel = new();


    public LogsPage()
    {
        DataContext = _logsViewModel;
        InitializeComponent();
       
    }
}



