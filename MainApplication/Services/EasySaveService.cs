using System.Diagnostics;
using System.Globalization;
using MainApplication.Handlers;
using MainApplication.Localization;
using MainApplication.Objects;

namespace MainApplication.Services;

internal class EasySaveService
{
    private static readonly EasySaveService Instance = new();
    private readonly ConfigurationService _configurationService = ConfigurationService.GetInstance();

    public List<CultureInfo> AllCultureInfo;


    private readonly string[] _jobApplicationProcess = {"CalculatorApp", "calc"};

    public CultureInfo SelectedCultureInfo;

    private EasySaveService()
    {
        var config = _configurationService.Config;
        Language.Culture = CultureInfo.GetCultureInfo(config.Language);
        SelectedCultureInfo = Language.Culture;
        AllCultureInfo = new List<CultureInfo>();
        foreach (var language in LanguageCheck.Languages)
            AllCultureInfo.Add(CultureInfo.GetCultureInfo(language));
    }

    public Action<Action>? DispatchUiAction { get; set; }

    public static EasySaveService GetInstance()
    {
        return Instance;
    }

    internal void ChangeCulture(CultureInfo language)
    {
        SelectedCultureInfo = language;
        Language.Culture = language;
        _configurationService.Config.Language = language.Name;
        _configurationService.SaveCurrentConfig();
    }


    internal bool JobApplicationIsRunning(CommandHandler origin)
    {
        var process = Array.Find(Process.GetProcesses(),
            (process) => _jobApplicationProcess.Contains(process.ProcessName));
        if (process == null)
            return false;
        process.EnableRaisingEvents = true;
        process.Exited += (object? sender, EventArgs e) => DispatchUiAction?.Invoke(origin.RaiseCanExecuteChanged);


        return true;
    }
}