using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public class DifferentialSave : ASave
{
    public DifferentialSave(Save save) : base(save)
    {
    }

    /// <summary>
    ///     Retrieve all file to copy
    /// </summary>
    /// <returns>true if Success</returns>
    protected override bool RetrieveFilesToCopy()
    {
        var files = GetAllFolderFiles(Save.SourcePath);

        if (files.Length <= 0)
            return false;
        var localsourcepath = Save.SourcePath.LocalPath;
        var localtargetpath = Save.TargetPath.LocalPath;

        foreach (var file in files)
        {
            var filefolder = file.Replace(localsourcepath, "");
            var path = Path.GetDirectoryName(file);
            if (path == null)
                continue;
            var fileName = Path.GetFileName(file);
            var targetfile = localtargetpath + filefolder;
            if (ToolService.FileCompare(file, targetfile)) continue;
            Console.WriteLine(file);
            var sourceFileInfo = new FileInfo(file);
            SaveFiles.Add(new SaveFile(path, fileName, sourceFileInfo.Length));
        }

        return true;
    }
}