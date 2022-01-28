using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

/// <summary>
/// This class handles the creation of the JSON log file, you can find the different types of information present
/// in the JSON file such as the name of the file, its location, the desired location for the save, the file size.... 
/// </summary>


public class Log  
{
   
    public string? Name { get; set; } 
    public string? SourcePath { get; set; }
    public string? TargetPath { get; set; }

    public int? FileSize { get; set; }
    public double? FileTransferTime { get; set; }
    public DateTime? Date { get; set; }

    public Log(string name, string sourcePath, string targetPath, int fileSize, double fileTransferTime, DateTime date)
    {
        Name = name;
        SourcePath = sourcePath;
        TargetPath = targetPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        Date = date;
    }
}