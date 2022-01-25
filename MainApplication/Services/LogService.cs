using MainApplication.Objects;
using MainApplication.Storages;

namespace MainApplication.Services;

public sealed class LogService
{
    private static readonly LogService Instance = new();
    public string? Name { get; set; }
    
    private const string LogsPath = "logs.json";

    private readonly IStorage<Log> _storage;

    private LogService()
    {
        _storage = new JsonStorage<Log>(LogsPath);
        LoadLogsFile();
    }

    private static void LoadLogsFile()
    {
        if (File.Exists(LogsPath)) return;
        using var sw = File.CreateText(LogsPath);
        sw.Close();
    }

    private void InsertLog(Log log)
    {
        _storage.AddNewElementWithoutRewrite(log);
    }

    public static LogService GetInstance()
    {
        return Instance;
    }
}