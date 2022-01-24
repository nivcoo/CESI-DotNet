namespace MainApplication.Services;

public sealed class LogService
{
    private static readonly LogService Instance = new();
    public string? Name { get; set; }

    private LogService()
    {
        LoadLogsFile();
    }
    
    private static void LoadLogsFile()
    {
        const string path = "saves.json";

        if (File.Exists(path)) return;
        using var sw = File.CreateText(path);
        sw.Close();
    }

    public static LogService GetInstance()
    {
        return Instance;
    }
}