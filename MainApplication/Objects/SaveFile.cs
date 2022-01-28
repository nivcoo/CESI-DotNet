namespace MainApplication.Objects;

public class SaveFile
{
    public string Path;
    public string FileName;
    public long FileSize;

    public SaveFile(string path, string fileName, long fileSize)
    {
        Path = path;
        FileName = fileName;
        FileSize = fileSize;
    }
}