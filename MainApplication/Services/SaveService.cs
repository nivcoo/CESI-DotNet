using MainApplication.Objects;
using MainApplication.Storages;

namespace MainApplication.Services;

public sealed class SaveService
{
    private static readonly SaveService Instance = new();
    public string? Name { get; set; }

    private readonly string _savesPath;

    private readonly IStorage<Save> _storage;

    private SaveService()
    {
        _savesPath = @"datas\saves.json";
        _storage = new JsonStorage<Save>(_savesPath);
        LoadSavesFile();
    }

    private void LoadSavesFile()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_savesPath) ?? string.Empty);
        if (!File.Exists(_savesPath))
            File.CreateText(_savesPath).Close();
    }

    public static SaveService GetInstance()
    {
        return Instance;
    }
}