using System.Text.Json.Serialization;
using MainApplication.Storages.Converter;

namespace MainApplication.Objects;

public class Log
{
    public string Name { get; set; }
    public Uri SourcePath { get; set; }
    public Uri TargetPath { get; set; }
    public long FileSize { get; set; }
    long FileTransferTime { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }

    public Log(string name, Uri sourcePath, Uri targetPath, long fileSize, long fileTransferTime, DateTime date)
    {
        Name = name;
        SourcePath = sourcePath;
        TargetPath = targetPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        Date = date;
    }
}