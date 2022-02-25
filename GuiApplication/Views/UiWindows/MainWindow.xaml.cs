using GuiApplication.Views.Pages;
using MainApplication.ViewModels;
using MainApplication.ViewModels.Home;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WinRT.Interop;

namespace GuiApplication.Views.UiWindows;

public sealed partial class MainWindow : Window
{
    private static readonly MainWindow Instance = new();
    private readonly App App = App.Instance;
    public IntPtr HWnd;
    private AppWindow _apw;

    public new DispatcherQueue DispatcherQueue;

    public NavigationView CurrentNavigationView;

    private MainWindowViewModel _mainWindowViewModel;


    public MainWindow()
    {
        _mainWindowViewModel = new MainWindowViewModel();
        InitializeComponent();
        GetAppWindowAndPresenter();
        _apw.Resize(new Windows.Graphics.SizeInt32 { Width = 1600, Height = 900 });
        _apw.Title = "EasySave";
        InitTexts();

        SetUIThread();

        

        CurrentNavigationView = MainNavigationView;
    }

    public AHomeViewModel GetHomeViewModel()
    {
        if(App.IsServer)
            return new ServerHomeViewModel();
        return new ClientHomeViewModel();
    }

    public void SetUIThread()
    {
        DispatcherQueue = DispatcherQueue.GetForCurrentThread();
    }

    public void InitTexts()
    {
        PageHomeNavigationItem.Content = MainApplication.Localization.Language.PAGE_NAVIGATION_HOME_TITLE;
        PageSavesNavigationItem.Content = MainApplication.Localization.Language.PAGE_NAVIGATION_SAVES_TITLE;

        PageLogsNavigationItem.Content = MainApplication.Localization.Language.PAGE_NAVIGATION_LOGS_TITLE;
    }

    public void GetAppWindowAndPresenter()
    {
        HWnd = WindowNative.GetWindowHandle(this);
        WindowId myWndId = Win32Interop.GetWindowIdFromWindow(HWnd);
        _apw = AppWindow.GetFromWindowId(myWndId);
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

            case "LogsPage":
                sender.Header = MainApplication.Localization.Language.PAGE_LOGS_TITLE;
                MainContentFrame.Navigate(typeof(LogsPage));
                break;
        }

    }

    public void SelectSocketType(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton rb)
        {
            string socketType = rb.Tag.ToString();
            Debug.WriteLine(socketType);

            switch (socketType)
            {
                case "type_server":
                    App.IsServer = true;
                    break;
                case "type_client":
                    App.IsServer = false;
                    break;
            }
            _mainWindowViewModel.InitSocket(App.IsServer);
            SelectTypeView.Visibility = Visibility.Collapsed;
            MainNavigationView.Visibility = Visibility.Visible;
        }
    }

    public static MainWindow GetInstance()
    {
        return Instance;
    }
}

