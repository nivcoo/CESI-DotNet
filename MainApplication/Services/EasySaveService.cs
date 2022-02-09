using MainApplication.Localization;
using System.Diagnostics;
using System.Globalization;

namespace MainApplication.Services;

internal class EasySaveService
{
    private static readonly EasySaveService Instance = new();

    public List<CultureInfo> AllCultureInfo;

    public CultureInfo SelectedCultureInfo;

    public Action<Action>? DispatchUiAction { get; set; }

    private string[] JobApplicationProcess = { "CalculatorApp", "calc" };

    public EasySaveService() {

        Language.Culture = CultureInfo.GetCultureInfo("fr-FR");
        SelectedCultureInfo = Language.Culture;
        AllCultureInfo = new List<CultureInfo>();
        foreach (string language in LanguageCheck.Languages)
            AllCultureInfo.Add(CultureInfo.GetCultureInfo(language));
    }


    public static EasySaveService GetInstance()
    {
        return Instance;
    }

    internal void ChangeCulture(CultureInfo language)
    {
        SelectedCultureInfo = language;
        Language.Culture = language;
    }

    internal bool JobApplicationIsRunning(Handlers.CommandHandler origin)
    {

        var process = Array.Find(Process.GetProcesses(), (process) => JobApplicationProcess.Contains(process.ProcessName));
        if (process == null)
            return false;

        process.EnableRaisingEvents = true;

        process.Exited -= (object? sender, EventArgs e) => DispatchUiAction?.Invoke(() =>
        {
            origin.RaiseCanExecuteChanged();
        });
        process.Exited += (object? sender, EventArgs e) => DispatchUiAction?.Invoke(() =>
        {
            origin.RaiseCanExecuteChanged();
        });

        
        return true;
    }
}