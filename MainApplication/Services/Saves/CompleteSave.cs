using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public class CompleteASave : ASave
{
    public CompleteASave(Save save) : base(save)
    {
    }

    protected override bool RetrieveFilesToCopy()
    {
        var files = GetAllFolderFiles(Save.SourcePath);
        if (files.Length <= 0)
            return false;

        foreach (var file in files)
        {
            var path = Path.GetDirectoryName(file);
            if (path == null)
                continue;
            var fileName = Path.GetFileName(file);
            var sourceFileInfo = new FileInfo(file);
            SaveFiles.Add(new SaveFile(path, fileName, sourceFileInfo.Length));
        }

        DeleteFilesBeforeCopy = true;

        return true;
    }
}