using MainApplication.Objects;

namespace MainApplication.Services;

public sealed class SaveService
{
    private static readonly SaveService Instance = new();
    public string? Name { get; set; }

    private SaveService()
    {
        LoadSavesFile();
    }

    private static void LoadSavesFile()
    {
        const string path = "saves.json";
        if (File.Exists(path)) return;
        using var sw = File.CreateText(path);
        sw.Close();
    }

    public static SaveService GetInstance()
    {
        return Instance;
    }
}