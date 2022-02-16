using System.Text.Json.Serialization;
using MainApplication.Storages.Converter;

namespace MainApplication.Objects;

/// <summary>
///     Structure of the JSON LogFile : /// This  handles the creation of the JSON log file, you can find the different
///     types of information present
///     in the JSON file such as the name of the file, its location, the desired location for the save, the file size....
/// </summary>
public class Log
{
    public Log(string name, Uri sourcePath, Uri targetPath, long fileSize, long fileTransferTime, double encryptTime,
        DateTime date)
    {
        Name = name;
        SourcePath = sourcePath;
        TargetPath = targetPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        EncryptTime = encryptTime;
        Date = date;
    }

    public string Name { get; set; }
    public Uri SourcePath { get; set; }
    public Uri TargetPath { get; set; }
    public long FileSize { get; set; }
    public long FileTransferTime { get; set; }

    public double EncryptTime { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
}