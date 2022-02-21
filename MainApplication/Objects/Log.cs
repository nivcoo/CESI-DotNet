using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MainApplication.Storages.Converter;

namespace MainApplication.Objects;

/// <summary>
///     Structure of the JSON LogFile : /// This  handles the creation of the JSON log file, you can find the different
///     types of information present
///     in the JSON file such as the name of the file, its location, the desired location for the save, the file size....
/// </summary>
public class Log
{
    private Log()
    {
        Name = "";
        SourcePath = new Uri(@"C:\EasySave");
        TargetPath = new Uri(@"C:\EasySave");
        FileSize = 0;
        FileTransferTime = 0;
        EncryptTime = 0;
        Date = DateTime.Now;
    }
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

    [XmlIgnore]
    public Uri SourcePath { get; set; }

    [XmlAttribute("SourcePath")]
    public string SourcePathString
    {
        get => SourcePath.ToString();
        set => SourcePath = new Uri(value);
    }

    [XmlIgnore]
    public Uri TargetPath { get; set; }

    [XmlAttribute("TargetPath")]
    public string TargetPathString
    {
        get => TargetPath.ToString();
        set => TargetPath = new Uri(value);
    }
    public long FileSize { get; set; }
    public long FileTransferTime { get; set; }

    public double EncryptTime { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
}