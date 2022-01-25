using MainApplication.Objects;
using MainApplication.Storages;

namespace MainApplication.Services;

public sealed class LogService
{
    private static readonly LogService Instance = new();
    
    private readonly string _logsPath;

    private readonly IStorage<Log> _storage;

    private LogService()
    {
        _logsPath = @"data\logs.json";
        _storage = new JsonStorage<Log>(_logsPath);
        LoadLogsFile();
    }

    private void LoadLogsFile()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_logsPath) ?? string.Empty);
        if (!File.Exists(_logsPath))
            File.CreateText(_logsPath).Close();
    }

    public void InsertLog(Log log)
    {
        _storage.AddNewElementWithoutRewrite(log);
    }

    public static LogService GetInstance()
    {
        return Instance;
    }
}