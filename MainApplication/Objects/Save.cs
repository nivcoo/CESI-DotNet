using MainApplication.Objects.Enums;

namespace MainApplication.Objects;


/// <summary>
/// Just like the log file creation, this creates the JSON SAVE LOG file
/// </summary>
public class Save
{
    public string? Name { get; set; }
    public string? SourcePath { get; set; }
    public string? TargetPath { get; set; }
    
    public TypeSave? Type { get; set; }
    public State? State { get; set; }
    public int? TotalFilesToCopy { get; set; }
    public int? TotalFilesSize { get; set; }
    public int? NbFilesLeftToDo { get; set; }
    public int? Progression { get; set; }

    public Save(string name, string sourcePath, string targetPath, TypeSave type, State state, int totalFilesToCopy,
        int totalFilesSize, int nbFilesLeftToDo, int progression)
    {
        Name = name;
        SourcePath = sourcePath;
        TargetPath = targetPath;
        Type = type;
        State = state;
        TotalFilesToCopy = totalFilesToCopy;
        TotalFilesSize = totalFilesSize;
        NbFilesLeftToDo = nbFilesLeftToDo;
        Progression = progression;
    }
}