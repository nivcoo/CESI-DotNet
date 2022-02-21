using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

/// <summary>
///     Structure of the JSON ConfigFile : /// This  handles the creation of the JSON log file, you can find the different
///     types of information present
/// </summary>
public class Config
{
    public Config(string language, FileType savesFileType, FileType logsFileType, List<string> encryptExtensions,
        List<string> priorityFiles, double maxFileSize)
    {
        Language = language;
        SavesFileType = savesFileType;
        LogsFileType = logsFileType;
        EncryptExtensions = encryptExtensions;
        PriorityFiles = priorityFiles;
        MaxFileSize = maxFileSize;
    }

    public string Language { get; set; }
    
    public FileType SavesFileType { get; set; }

    public FileType LogsFileType { get; set; }

    public List<string> EncryptExtensions { get; set; }

    public List<string> PriorityFiles { get; set; }

    public double MaxFileSize { get; set; }
}
