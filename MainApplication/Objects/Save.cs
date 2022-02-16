using MainApplication.Handlers;
using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

/// <summary>
///     Just like the log file creation, this creates the JSON SAVE LOG file
/// </summary>
public class Save : INPChanged
{
    private int _filesAlreadyDone;


    private int _nbFilesLeftToDo;

    private double _progression;

    private State _state;

    private int _totalFilesToCopy;

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

    public string Name { get; }

    public Uri SourcePath { get; set; }

    public Uri TargetPath { get; set; }

    public TypeSave Type { get; set; }

    public State State
    {
        get => _state;
        set => SetField(ref _state, value, nameof(State));
    }

    public int TotalFilesToCopy
    {
        get => _totalFilesToCopy;
        set => SetField(ref _totalFilesToCopy, value, nameof(TotalFilesToCopy));
    }

    public long TotalFilesSize { get; set; }

    public int NbFilesLeftToDo
    {
        get => _nbFilesLeftToDo;
        set => SetField(ref _nbFilesLeftToDo, value, nameof(NbFilesLeftToDo));
    }

    public int FilesAlreadyDone
    {
        get => _filesAlreadyDone;
        set => SetField(ref _filesAlreadyDone, value, nameof(FilesAlreadyDone));
    }

    public double Progression
    {
        get => _progression;
        set => SetField(ref _progression, value, nameof(Progression));
    }

    public void ResetValues()
    {
        State = State.End;
        NbFilesLeftToDo = 0;
        FilesAlreadyDone = 0;
        TotalFilesToCopy = 0;
        Progression = 0;
    }
/// <summary>
/// Shows the progression of the save
/// </summary>
///
    public void UpdateProgression()
    {
        Progression = TotalFilesToCopy == 0 ? 0 : (TotalFilesToCopy - NbFilesLeftToDo) * 100 / TotalFilesToCopy;
    }
}