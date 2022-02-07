﻿using MainApplication.Handlers;
using MainApplication.Objects.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MainApplication.Objects;


/// <summary>
/// Just like the log file creation, this creates the JSON SAVE LOG file
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
        get
        {
            return _state;
        }
        set => SetField(ref _state, value, nameof(State));
    }

    [JsonIgnore]
    public bool IsStarted { get => State == State.Active; }

    [JsonIgnore]
    public bool IsStopped { get => State == State.End; }

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

    public void UpdateProgression()
    {
        Progression = TotalFilesToCopy == 0 ? 0 :(TotalFilesToCopy - NbFilesLeftToDo) * 100 / TotalFilesToCopy;
    }
}