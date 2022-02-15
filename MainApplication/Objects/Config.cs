using System.Text.Json.Serialization;
using MainApplication.Objects.Enums;
using MainApplication.Storages.Converter;

namespace MainApplication.Objects;

/// <summary>
///  Structure of the JSON ConfigFile : /// This  handles the creation of the JSON log file, you can find the different types of information present
/// </summary>
public class Config
{
    public string Language { get; set; }
    public SaveFileType SaveFileType { get; set; }
    public string[] EncryptExtensions { get; set; }

    public Config(string language, SaveFileType saveFileType, string[] encryptExtensions)
    {
        Language = language;
        SaveFileType = saveFileType;
        EncryptExtensions = encryptExtensions;
    }
}