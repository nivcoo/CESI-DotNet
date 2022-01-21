namespace MainApplication.Objects;

public class Save
{
    public string? Name { get; set; }
    public string? SourceFilePath { get; set; }
    public string? TargetFilePath { get; set; }
    public string? State { get; set; }
    public int? TotalFilesToCopy { get; set; }
    public int? TotalFilesSize { get; set; }
    public int? NbFilesLeftToDo { get; set; }
    public int? Progression { get; set; }

    public Save(string name, string sourceFilePath, string targetFilePath, string state, int totalFilesToCopy,
        int totalFilesSize, int nbFilesLeftToDo, int progression)
    {
        Name = name;
        SourceFilePath = sourceFilePath;
        TargetFilePath = targetFilePath;
        State = state;
        TotalFilesToCopy = totalFilesToCopy;
        TotalFilesSize = totalFilesSize;
        NbFilesLeftToDo = nbFilesLeftToDo;
        Progression = progression;
    }
}