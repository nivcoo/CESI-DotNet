using System.Text.Json.Serialization;
using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

public class Save
{
    [JsonPropertyName("Name")] public string Name { get; }

    [JsonPropertyName("SourcePath")] public Uri SourcePath { get; set; }

    [JsonPropertyName("TargetPath")] public Uri TargetPath { get; set; }

    [JsonPropertyName("Type")] public TypeSave Type { get; set; }

    [JsonPropertyName("State")] public State State { get; set; }

    [JsonPropertyName("TotalFilesToCopy")] public int TotalFilesToCopy { get; set; }

    [JsonPropertyName("TotalFilesSize")] public long TotalFilesSize { get; set; }

    [JsonPropertyName("NbFilesLeftToDo")] public int NbFilesLeftToDo { get; set; }

    [JsonPropertyName("Progression")] public int Progression { get; set; }

    [JsonConstructor]
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