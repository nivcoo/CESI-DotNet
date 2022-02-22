using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Storages;
using System.Diagnostics;

namespace MainApplication.Services;

internal sealed class ConfigurationService
{
    private static readonly ConfigurationService Instance = new();

    private readonly string localPath;

    private string? _configPath;

    private AStorage<Config>? _storage;

    public Config Config;

    public ConfigurationService()
    {
        localPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"\Cesi-EasySave\");
        if (!Directory.Exists(localPath))
            Directory.CreateDirectory(localPath);
        Config = new Config("en-US", FileType.JSON, FileType.JSON, new List<string>(), new List<string>(), 100);
        LoadConfigFile();
    }

    private void LoadConfigFile()
    {
        _configPath = localPath + @"\data\config.json";

        Directory.CreateDirectory(Path.GetDirectoryName(_configPath) ?? string.Empty);
        if (!File.Exists(_configPath))
            File.CreateText(_configPath).Close();
        _storage = new JsonStorage<Config>(_configPath);

        var config = _storage.GetElement();
        if (config == default)
            SaveCurrentConfig();
        else
            Config = config;
    }

    public void SaveCurrentConfig()
    {
        if (Config != null)
            _storage?.WriteElement(Config);
    }

    internal void ChangeSavesFileType(FileType savesFileType)
    {
        
        if (savesFileType == Config.SavesFileType)
            return;
        SaveService ss = SaveService.GetInstance();
        Config.SavesFileType = savesFileType;
        ss.LoadSavesFile();
        ss.InitSavesList();
        SaveCurrentConfig();
    }

    internal void ChangeLogsFileType(FileType logsFileType)
    {
        Config.LogsFileType = logsFileType;
        SaveCurrentConfig();
    }

    public static ConfigurationService GetInstance()
    {
        return Instance;
    }

    public bool AlreadyEncryptExtensionWithSameName(string extensionName)
    {
        return Config.EncryptExtensions.Contains(extensionName);
    }

    internal void ChangeMaxFileSize(double maxFileSize)
    {
        Config.MaxFileSize = maxFileSize;
        SaveCurrentConfig();
    }

    public bool AlreadyPriorityFileWithSameName(string fileName)
    {
        return Config.PriorityFiles.Contains(fileName);
    }


    public bool AddEncryptExtension(string extension)
    {
        if (AlreadyEncryptExtensionWithSameName(extension))
            return false;
        Config.EncryptExtensions.Add(extension);
        SaveCurrentConfig();
        return true;
    }

    public bool AddPriorityFile(string file)
    {
        if (AlreadyPriorityFileWithSameName(file))
            return false;
        Config.PriorityFiles.Add(file);
        SaveCurrentConfig();
        return true;
    }


    public bool RemoveEncryptExtension(string extension)
    {
        Config.EncryptExtensions.Remove(extension);
        SaveCurrentConfig();
        return true;
    }

    public bool RemovePriorityFile(string file)
    {
        Config.PriorityFiles.Remove(file);
        SaveCurrentConfig();
        return true;
    }
}
