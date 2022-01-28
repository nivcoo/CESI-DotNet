using System.Security.Cryptography;
using MainApplication.Annotations;
using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public class DifferentialSave : ASave
{
    public DifferentialSave(Save save) : base(save)
    {
    }

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
            var hash = GetHashSha256(file);
            var targetfile = localtargetpath+ filefolder;
            if (!IsSameFile(hash, GetHashSha256(targetfile)))
            {
                Console.WriteLine(file);
                    var sourceFileInfo = new FileInfo(file);
                SaveFiles.Add(new SaveFile(path, fileName, hash, sourceFileInfo.Length));
            }
        }

        return true;
    }

    protected override bool CopyFiles()
    {
        if (SaveFiles.Count <= 0)
            return false;
        var sourceLocalPath = Save.SourcePath.LocalPath;
        var targetLocalPath = Save.TargetPath.LocalPath;
        DeleteFolderWithFiles(Save.TargetPath);
        foreach (var saveFile in SaveFiles)
        {
            var actualTimestamp = ToolService.GetTimestamp();
            var sourceFolder = saveFile.Path;
            var localPath = sourceFolder.Replace(sourceLocalPath, "");
            var targetFolder = targetLocalPath + localPath;
            Directory.CreateDirectory(targetFolder);
            var fileName = saveFile.FileName;
            var sourceFilePath = Path.Combine(sourceFolder, fileName);
            var targetFilePath = Path.Combine(targetFolder, fileName);
            File.Copy(sourceFilePath, targetFilePath, true);
            UpdateSaveStatut();
            var sourceFileInfo = new FileInfo(sourceFilePath);
            var finalTimestamp = ToolService.GetTimestamp();
            var time = finalTimestamp - actualTimestamp;
            LogService.InsertLog(new Log(Save.Name, new Uri(sourceFilePath), new Uri(targetFilePath),
                sourceFileInfo.Length, time, DateTime.Now));
        }

        return true;
    }

    private static bool IsSameFile(IEnumerable<byte> sourceFile, IEnumerable<byte> destinationFile)
    {
        return ToolService.BytesToString(sourceFile) == ToolService.BytesToString(destinationFile);
    }
}