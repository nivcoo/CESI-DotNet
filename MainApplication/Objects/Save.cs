﻿using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

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

    public int Progression { get; set; }

    public Save(string name, Uri sourcePath, Uri targetPath, TypeSave type, State state, int totalFilesToCopy,
        long totalFilesSize, int nbFilesLeftToDo, int progression)
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
        Progression = (TotalFilesToCopy - NbFilesLeftToDo) * 100 / TotalFilesToCopy;
    }
}