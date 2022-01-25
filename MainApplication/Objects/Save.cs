using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

public class Save
{
    public string? Name { get; set; }
    public Uri? SourcePath { get; set; }
    public Uri? TargetPath { get; set; }

    public TypeSave? Type { get; set; }
    public State? State { get; set; }
    public int? TotalFilesToCopy { get; set; }
    public int? TotalFilesSize { get; set; }
    public int? NbFilesLeftToDo { get; set; }
    public int? Progression { get; set; }
}