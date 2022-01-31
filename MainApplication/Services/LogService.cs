using System.Globalization;
using MainApplication.Objects;
using MainApplication.Storages;

namespace MainApplication.Services;

internal sealed class LogService
{
    private static readonly LogService Instance = new();

    private string? _logsPath;

    private AStorage<Log>? _storage;

    private void LoadLogsFile()
    {
        var dateTime = DateTime.Now;
        var date = dateTime.ToString("yyyy-MM-dd");
        _logsPath = @"data\logs\" + date + ".log.json";
        _storage = new JsonStorage<Log>(_logsPath);
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