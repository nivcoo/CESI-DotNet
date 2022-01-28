namespace MainApplication.Services;

/// <summary>
/// This service manges the LogFile module
/// </summary>
public sealed class LogService
{
    private static readonly LogService Instance = new();
    public string? Name { get; set; }

    private LogService()
    {
        /*var log = new Log("test1", "test2");
        Console.WriteLine(log.Name);*/
    }

    public static LogService GetInstance()
    {
        return Instance;
    }
}