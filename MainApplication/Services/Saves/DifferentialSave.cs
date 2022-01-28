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
        throw new NotImplementedException();
    }

    protected override bool CopyFiles()
    {
        if (SaveFiles.Count <= 0)
            return false;
        var sourceLocalPath = Save.SourcePath.LocalPath;
        var targetLocalPath = Save.TargetPath.LocalPath;

        foreach (var saveFile in SaveFiles)
        {
            var actualTimestamp = ToolService.GetTimestamp();
            var sourceFolder = saveFile.Path;
            var localPath = sourceFolder.Replace(sourceLocalPath, "");
            var targetFolder = targetLocalPath + localPath;
            var fileName = saveFile.FileName;

            var targetFilePath = Path.Combine(targetFolder, fileName);


            if (IsSameFile(saveFile.Hash, GetHashSha256(targetFilePath)))
                continue;
            File.Delete(targetFilePath);
            Directory.CreateDirectory(targetFolder);
            var sourceFilePath = Path.Combine(sourceFolder, fileName);
            File.Copy(sourceFilePath, targetFilePath);
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