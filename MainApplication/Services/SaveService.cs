using MainApplication.Objects;
using MainApplication.Storages;

namespace MainApplication.Services;

public sealed class SaveService
{
    private static readonly SaveService Instance = new();
    public string? Name { get; set; }
    
    private const string SavesPath = "saves.json";

    private readonly IStorage<Save> _storage;
    
    private SaveService()
    {
        _storage = new JsonStorage<Save>(SavesPath);
        LoadSavesFile();
    }

    private static void LoadSavesFile()
    {
        if (File.Exists(SavesPath)) return;
        using var sw = File.CreateText(SavesPath);
        sw.Close();
    }

    public static SaveService GetInstance()
    {
        return Instance;
    }
}