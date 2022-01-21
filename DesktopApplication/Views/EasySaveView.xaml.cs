using System.Windows;
using MainApplication.ViewModels;

namespace DesktopApplication.Views
{
    /// <summary>
    /// Interaction logic for EasySaveView.xaml
    /// </summary>
    public partial class EasySaveView
    {
        
        private readonly MainViewModel _mainViewModel = new ();
        public EasySaveView()
        {
            InitializeComponent();
            DataContext = _mainViewModel;
        }
    }
}