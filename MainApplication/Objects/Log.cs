namespace MainApplication.Objects;

public class Log
{
    public string? Name { get; set; }
    public string? Path { get; set; }

    public Log(string name, string path)
    {
        Name = name;
        Path = path;
    }
}