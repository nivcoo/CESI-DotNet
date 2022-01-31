using System.Text.Json.Serialization;
using MainApplication.Storages.Converter;

namespace MainApplication.Objects;

public class Log
{
    private string? Name { get; set; }
    private Uri SourcePath { get; set; }
    private Uri TargetPath { get; set; }
    private long FileSize { get; set; }
    long FileTransferTime { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    private DateTime Date { get; set; }

    public Log(string? name, Uri sourcePath, Uri targetPath, long fileSize, long fileTransferTime, DateTime date)
    {
        Name = name;
        SourcePath = sourcePath;
        TargetPath = targetPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        Date = date;
    }
}