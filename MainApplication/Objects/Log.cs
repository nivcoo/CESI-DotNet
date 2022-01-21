using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

public class Log
{
    public string? Name { get; set; }
    public string? SourcePath { get; set; }
    public string? TargetPath { get; set; }

    public int? FileSize { get; set; }
    public double? FileTransferTime { get; set; }
    public DateTime? Time { get; set; }

    public Log(string name, string sourcePath, string targetPath, int fileSize, double fileTransferTime, DateTime time)
    {
        Name = name;
        SourcePath = sourcePath;
        TargetPath = targetPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        Time = time;
    }
}