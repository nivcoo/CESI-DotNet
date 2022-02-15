using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Storages;

namespace MainApplication.Services;

/// <summary>
/// Log Manager
/// </summary>
internal sealed class LogService
{
    private static readonly LogService Instance = new();
    private readonly ConfigurationService ConfigurationService = ConfigurationService.GetInstance();

    private string? _logsPath;

    private AStorage<Log>? _storage;

    private void LoadLogsFile()
    {
        var dateTime = DateTime.Now;
        var date = dateTime.ToString("yyyy-MM-dd");
        if (ConfigurationService.Config.SaveFileType == SaveFileType.XML)
        {
            _logsPath = AppDomain.CurrentDomain.BaseDirectory + @"data\logs\" + date + ".log.xml";
            _storage = new JsonStorage<Log>(_logsPath);
        }
        else
        {
            _logsPath = AppDomain.CurrentDomain.BaseDirectory + @"data\logs\" + date + ".log.json";
            _storage = new JsonStorage<Log>(_logsPath);
        }
        Directory.CreateDirectory(Path.GetDirectoryName(_logsPath) ?? string.Empty);
        if (!File.Exists(_logsPath))
            File.CreateText(_logsPath).Close();
    }
    
    /// <summary>
    /// Insert log object into storage
    /// </summary>
    /// <param name="log"></param>
    public void InsertLog(Log log)
    {
        LoadLogsFile();
        _storage?.AddNewElementWithoutRewrite(log);
    }

    public static LogService GetInstance()
    {
        return Instance;
    }
}