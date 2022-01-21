namespace MainApplication.Objects;

public class Log
{
    public string? Name { get; set; }
    public string? FileSource { get; set; }
    public string? FileTarget { get; set; }
    public string? DestPath { get; set; }
    public int? FileSize { get; set; }
    public double? FileTransferTime { get; set; }
    public string? Time { get; set; }

    public Log(string name, string fileSource, string fileTarget, string destPath, int fileSize,
        double fileTransferTime, string time)
    {
        Name = name;
        FileSource = fileSource;
        FileTarget = fileTarget;
        DestPath = destPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        Time = time;
    }
}