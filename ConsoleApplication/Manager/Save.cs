namespace ConsoleApplication.Manager;

public class Save
{
    private string? Name;
    private string? Path;

    public Save(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public string? GetName()
    {
        return Name;
    }

    public string? GetPath()
    {
        return Path;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetPath(string path)
    {
        Path = path;
    }
}