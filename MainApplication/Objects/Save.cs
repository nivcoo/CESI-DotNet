using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

/// <summary>
/// Structure of the save : Name, Source path etc..... 
/// </summary>

public class Save
{
    public string Name { get; }

    public Uri SourcePath { get; set; }

    public Uri TargetPath { get; set; }

    public TypeSave Type { get; set; }

    public State State { get; set; }

    public int TotalFilesToCopy { get; set; }

    public long TotalFilesSize { get; set; }

    public int NbFilesLeftToDo { get; set; }

    public double Progression { get; set; }

    public Save(string name, Uri sourcePath, Uri targetPath, TypeSave type, State state, int totalFilesToCopy,
        long totalFilesSize, int nbFilesLeftToDo, double progression)
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
/// <summary>
/// Shows the progression of the save
/// </summary>
///
    public void UpdateProgression()
    {
        Progression = TotalFilesToCopy == 0 ? 0 :(TotalFilesToCopy - NbFilesLeftToDo) * 100 / TotalFilesToCopy;

    }
}