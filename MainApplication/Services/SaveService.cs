using MainApplication.Objects;

namespace MainApplication.Services;

public sealed class SaveService
{
    private static readonly SaveService Instance = new ();
    public string? Name { get; set; }

    private SaveService()
    {
        var save = new Save("test1", "test2");
        Console.WriteLine(save.Name);
    }

    public static SaveService GetInstance()
    {
        return Instance;
    }
}