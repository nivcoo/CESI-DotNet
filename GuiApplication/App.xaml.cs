using GuiApplication.Views;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GuiApplication;

public partial class App : Application
{
    public App()
    {
        Process proc = Process.GetCurrentProcess();
        int count = new List<Process>(Process.GetProcesses()).FindAll(p =>
            p.ProcessName == proc.ProcessName).Count;

        if (count > 1)
        {
            Environment.Exit(0);
            Process.GetCurrentProcess().Kill();
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

