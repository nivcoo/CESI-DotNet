using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using WinRT.Interop;

namespace GuiApplication.Views;

public sealed partial class MainWindow : Window
{
    private static readonly MainWindow Instance = new();
    public IntPtr HWnd;
    private AppWindow _apw;
    private OverlappedPresenter _presenter;


    public MainWindow()
    {
        GetAppWindowAndPresenter();
        InitializeComponent();
        _presenter.IsResizable = false;
        _apw.Resize(new Windows.Graphics.SizeInt32 { Width = 1400, Height = 800 });
        _apw.Title = "EasySave";
        _presenter.IsMaximizable = false;
    }

    public void GetAppWindowAndPresenter()
    {
        HWnd = WindowNative.GetWindowHandle(this);
        WindowId myWndId = Win32Interop.GetWindowIdFromWindow(HWnd);
        _apw = AppWindow.GetFromWindowId(myWndId);
        _presenter = _apw.Presenter as OverlappedPresenter;


    }


    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
            
            var selectedItem = (NavigationViewItem)args.SelectedItem;
            string selectedItemTag = ((string)selectedItem.Tag);
            sender.Header = "Page " + selectedItemTag;
            string pageName = "GuiApplication.Views.Pages." + selectedItemTag;
            Type pageType = Type.GetType(pageName);
            MainContentFrame.Navigate(pageType);
            
    }

    public static MainWindow GetInstance()
    {
        return Instance;
    }
}

