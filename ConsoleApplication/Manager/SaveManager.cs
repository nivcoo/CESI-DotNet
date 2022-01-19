using System.Security.Authentication.ExtendedProtection;

namespace ConsoleApplication.Manager;

public sealed class SaveManager
{
    private static readonly SaveManager Instance = new SaveManager();
    private string? _name;

    private SaveManager()
    {
        var save = new Save("test1", "test2");
        Console.WriteLine(save.GetName());
    }

    public void SetName(string name)
    {
        _name = name;
    }
    
    public string? GetName()
    {
        return _name;
    }

    public static SaveManager GetInstance()
    {
        return Instance;
    }
}