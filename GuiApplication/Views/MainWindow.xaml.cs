using GuiApplication.Views.Pages;
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
        _apw.Resize(new Windows.Graphics.SizeInt32 { Width = 1600, Height = 900 });
        _apw.Title = "EasySave";
        _presenter.IsMaximizable = false;
        InitTexts();
    }

    public void InitTexts()
    {
        PageHomeNavigationItem.Content = MainApplication.Localization.Language.PAGE_NAVIGATION_HOME_TITLE;
        PageSavesNavigationItem.Content = MainApplication.Localization.Language.PAGE_NAVIGATION_SAVES_TITLE;
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

        var selectedItem = args.SelectedItem as NavigationViewItem;
        string selectedItemTag = selectedItem.Tag as string;

        switch (selectedItemTag)
        {
            case "HomePage":
                sender.Header = MainApplication.Localization.Language.PAGE_HOME_TITLE;
                MainContentFrame.Navigate(typeof(HomePage));
                break;
            case "SavesPage":
                sender.Header = MainApplication.Localization.Language.PAGE_SAVES_TITLE;
                MainContentFrame.Navigate(typeof(SavesPage));
                break;
        }

    }

    public static MainWindow GetInstance()
    {
        return Instance;
    }
}

