namespace MainApplication.Objects;

public class LogFile
{
    public string FileName { get; }
    public string FilePath { get; }


    public LogFile(string fileName, string filePath)
    {
        FileName = fileName;
        FilePath = filePath;
    }
}