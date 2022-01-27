using System.Security.Cryptography;
using MainApplication.Annotations;
using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public class DifferentialSave : ASave
{
    public DifferentialSave(Save save) : base(save)
    {
    }

    public override bool RetrieveFilesToCopy()
    {
        throw new NotImplementedException();
    }

    protected override void UpdateStartSaveStatut()
    {
        throw new NotImplementedException();
    }

    public override bool CopyFiles()
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
            

            if (!IsSameFile(saveFile.Hash,GetHash(Path.Combine(targetFolder, fileName))))
            {
                File.Delete(Path.Combine(targetFolder, fileName));
                Directory.CreateDirectory(targetFolder);
                var sourceFilePath = Path.Combine(sourceFolder, fileName);
                var targetFilePath = Path.Combine(targetFolder, fileName);
                File.Copy(sourceFilePath, targetFilePath);
                UpdateSaveStatut();
                var sourceFileInfo = new FileInfo(sourceFilePath);
                var finalTimestamp = ToolService.GetTimestamp();
                var time = finalTimestamp - actualTimestamp;
                LogService.InsertLog(new Log(Save.Name, new Uri(sourceFilePath), new Uri(targetFilePath),
                    sourceFileInfo.Length, time, DateTime.Now));
            }
        }

        return true;
    }
    
    public bool IsSameFile(byte[] sourceFile, byte[] destinationFile)
    {
        if (BytesToString(sourceFile) == BytesToString(destinationFile))
        {
            return true;
        }

        return false;
    }
    
    public byte[] GetHash(string filename)
    {
        SHA256 hash = SHA256.Create();
        using (FileStream stream = File.OpenRead(filename))
        {
            return hash.ComputeHash(stream);
        }
    }

    private static string BytesToString(byte[] fileHash)
    {
        string result = "";
        foreach (byte b in fileHash) result += b.ToString("x2");
        return result;
    }
    
    

}