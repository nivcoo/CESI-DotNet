namespace MainApplication.Objects;

public class SaveFile
{
    public readonly string Path;
    public readonly string FileName;
    public readonly long FileSize;

    public SaveFile(string path, string fileName, long fileSize)
    {
        Path = path;
        FileName = fileName;
        FileSize = fileSize;
    }
}