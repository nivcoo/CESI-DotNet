using System.Text.Json.Serialization;
using MainApplication.Storages.Converter;

namespace MainApplication.Objects;

public class Log
{
    [JsonPropertyName("Name")] public string Name { get; set; }
    [JsonPropertyName("SourcePath")] public Uri SourcePath { get; set; }
    [JsonPropertyName("TargetPath")] public Uri TargetPath { get; set; }
    [JsonPropertyName("FileSize")] public int FileSize { get; set; }
    [JsonPropertyName("FileTransferTime")] public double FileTransferTime { get; set; }

    [JsonPropertyName("Date")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }

    [JsonConstructor]
    public Log(string name, Uri sourcePath, Uri targetPath, int fileSize, double fileTransferTime, DateTime date)
    {
        Name = name;
        SourcePath = sourcePath;
        TargetPath = targetPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        Date = date;
    }
}