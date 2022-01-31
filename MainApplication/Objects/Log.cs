using System.Text.Json.Serialization;
using MainApplication.Storages.Converter;

namespace MainApplication.Objects;

/// <summary>
///  Structure of the JSON LogFile : /// This  handles the creation of the JSON log file, you can find the different types of information present
/// in the JSON file such as the name of the file, its location, the desired location for the save, the file size.... 
/// </summary>
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