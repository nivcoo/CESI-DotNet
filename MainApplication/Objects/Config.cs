using MainApplication.Handlers;
using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

/// <summary>
///     Structure of the JSON ConfigFile : /// This  handles the creation of the JSON log file, you can find the different
///     types of information present
/// </summary>
public class Config : INPChanged
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

    private string _language;

    public string Language
    {
        get => _language;
        set => SetField(ref _language, value, nameof(Language));
    }

    private FileType _savesFileType;

    public FileType SavesFileType
    {
        get => _savesFileType;
        set => SetField(ref _savesFileType, value, nameof(SavesFileType));
    }



    private FileType _logsFileType;

    public FileType LogsFileType
    {
        get => _logsFileType;
        set => SetField(ref _logsFileType, value, nameof(LogsFileType));
    }

    public List<string> EncryptExtensions { get; set; }

    public List<string> PriorityFiles { get; set; }

    public double _maxFileSize;

    public double MaxFileSize
    {
        get => _maxFileSize;
        set => SetField(ref _maxFileSize, value, nameof(MaxFileSize));
    }

}
