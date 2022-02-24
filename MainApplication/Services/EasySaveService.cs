using System.Diagnostics;
using System.Globalization;
using MainApplication.Handlers;
using MainApplication.Localization;

namespace MainApplication.Services;

internal class EasySaveService
{
    private static readonly EasySaveService Instance = new();
    private readonly ConfigurationService _configurationService;
    private readonly UIService _uiService;

    public List<CultureInfo> AllCultureInfo;


    private readonly string[] _jobApplicationProcess = {"CalculatorApp", "calc"};

    public CultureInfo SelectedCultureInfo;


    private EasySaveService()
    {
        _uiService = UIService.GetInstance();
        _configurationService = ConfigurationService.GetInstance();
        var config = _configurationService.Config;
        Language.Culture = CultureInfo.GetCultureInfo(config.Language);
        SelectedCultureInfo = Language.Culture;
        AllCultureInfo = new List<CultureInfo>();
        foreach (var language in LanguageCheck.Languages)
            AllCultureInfo.Add(CultureInfo.GetCultureInfo(language));
    }

    internal void RegisterToEvent(object notifyEasySaveServicePropertyChanged)
    {
        throw new NotImplementedException();
    }

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

    internal bool JobApplicationIsRunning(Action action)
    {
        var process = Array.Find(Process.GetProcesses(),
            (process) => _jobApplicationProcess.Contains(process.ProcessName));
        if (process == null)
            return false;
        process.EnableRaisingEvents = true;
        process.Exited += (object? sender, EventArgs e) => _uiService.DispatchUiAction?.Invoke(action);


        return true;
    }
}