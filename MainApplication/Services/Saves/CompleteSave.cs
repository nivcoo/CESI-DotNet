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
            var hash = GetHashSha256(file);
            var sourceFileInfo = new FileInfo(file);
            SaveFiles.Add(new SaveFile(path, fileName, hash, sourceFileInfo.Length));
        }

        return true;
    }

    protected override void UpdateStartSaveStatut()
    {
        Save.TotalFilesToCopy = SaveFiles.Count;
        Save.NbFilesLeftToDo = SaveFiles.Count;
        Save.TotalFilesSize = SaveFiles.Sum(saveFile => saveFile.FileSize);
        UpdateSaveStorage();
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
}