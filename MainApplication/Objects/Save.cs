using MainApplication.Handlers;
using MainApplication.Objects.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MainApplication.Objects;

/// <summary>
/// Structure of the save : Name, Source path etc..... 
/// </summary>
public class Save : INPChanged
{
    public string Name { get; }

    public Uri SourcePath { get; set; }

    public Uri TargetPath { get; set; }

    public TypeSave Type { get; set; }

    private State _state;

    public State State
    {
        get => _state;
        set => SetField(ref _state, value, nameof(State));
    }

    private int _totalFilesToCopy;

    public int TotalFilesToCopy
    {
        get => _totalFilesToCopy;
        set => SetField(ref _totalFilesToCopy, value, nameof(TotalFilesToCopy));
    }

    public long TotalFilesSize { get; set; }


    private int _nbFilesLeftToDo;

    public int NbFilesLeftToDo
    {
        get => _nbFilesLeftToDo;
        set {
            SetField(ref _nbFilesLeftToDo, value, nameof(NbFilesLeftToDo));
            FilesAlreadyDone = _totalFilesToCopy - value;
        }
    }
    private int _filesAlreadyDone;
    public int FilesAlreadyDone
    {
        get => _filesAlreadyDone;
        set => SetField(ref _filesAlreadyDone, value, nameof(FilesAlreadyDone));
    }

    private double _progression;

    public double Progression
    {
        get => _progression;
        set => SetField(ref _progression, value, nameof(Progression));
    }

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