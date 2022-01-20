namespace MainApplication.Objects;

public class Save
{
    public string? Name { get; set; }
    public string? Path { get; set; }

    public Save(string name, string path)
    {
        Name = name;
        Path = path;
    }
}