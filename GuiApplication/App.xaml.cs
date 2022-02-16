using GuiApplication.Views;
using Microsoft.UI.Xaml;
using System;
using System.Threading;

namespace GuiApplication;

public partial class App : Application
{
    public App()
    {
        using var mutex = new Mutex(false, "easysave Application");
        bool isAnotherInstanceOpen = !mutex.WaitOne(TimeSpan.Zero);
        if (isAnotherInstanceOpen)
        {
            Environment.Exit(0);
            return;
        }

        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = MainWindow.GetInstance();
        m_window.Activate();
    }

    private Window m_window;
}

