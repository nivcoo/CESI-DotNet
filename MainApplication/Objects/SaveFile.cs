namespace MainApplication.Objects;

public class SaveFile
{
    public string Path;
    public string FileName;
    public byte[] Hash;
    public long FileSize;

    public SaveFile(string path, string fileName, byte[] hash, long fileSize)
    {
        Path = path;
        FileName = fileName;
        Hash = hash;
        FileSize = fileSize;
    }
}