using System.Windows;
using MainApplication.ViewModels;

namespace DesktopApplication.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        
        private readonly MainViewModel _mainViewModel = new ();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _mainViewModel;
        }
    }
}