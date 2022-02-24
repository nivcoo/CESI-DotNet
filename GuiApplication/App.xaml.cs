using GuiApplication.Views.UiWindows;
using Microsoft.UI.Xaml;

namespace GuiApplication;

public partial class App : Application
{

    public static App Instance { get; private set; }

    public bool IsServer = true;

    public App()
    {
        Instance = this;
        /*Process proc = Process.GetCurrentProcess();
        int count = new List<Process>(Process.GetProcesses()).FindAll(p =>
            p.ProcessName == proc.ProcessName).Count;

        if (count > 1)
        {
            Environment.Exit(0);
            Process.GetCurrentProcess().Kill();
            return;
        }*/

        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Window = MainWindow.GetInstance();
        Window.Activate();
    }

    public Window Window;
}

