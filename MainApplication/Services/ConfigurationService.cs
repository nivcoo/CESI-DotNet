using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Storages;

namespace MainApplication.Services;

internal sealed class ConfigurationService
{

    private static readonly ConfigurationService Instance = new();

    public Config Config;

    private string? _configPath;

    private AStorage<Config>? _storage;

    public ConfigurationService() {
        Config = new Config("en-US", SaveFileType.JSON, Array.Empty<string>());
        LoadConfigFile();
    }

    private void LoadConfigFile()
    {
        _configPath = AppDomain.CurrentDomain.BaseDirectory + @"data\config.json";
        _storage = new JsonStorage<Config>(_configPath);
        Directory.CreateDirectory(Path.GetDirectoryName(_configPath) ?? string.Empty);
        if (!File.Exists(_configPath))
            File.CreateText(_configPath).Close();

        var config = _storage.GetElement();
        if (config == default)
        {
            SaveCurrentConfig();
        } else
            Config = config;
        
    }

    public void SaveCurrentConfig() 
    {

        
        if (Config != null)
        {
            _storage?.WriteElement(Config);
        }
    }

    internal void ChangeSaveFileType(SaveFileType saveFileType)
    {
        Config.SaveFileType = saveFileType;
        SaveCurrentConfig();
    }


    public static ConfigurationService GetInstance()
    {
        return Instance;
    }
}

