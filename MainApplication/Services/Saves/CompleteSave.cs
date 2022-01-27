using MainApplication.Annotations;
using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public class CompleteASave : ASave
{
    public CompleteASave(Save save) : base(save)
    {
    }

    public override bool RunSave()
    {
        var files = Directory.GetFiles(Save.SourcePath.LocalPath);
        if (files.Length <= 0)
            return false;
        
        foreach (var file in files)
        {
            Console.WriteLine(file);
        }

        return false;
    }
}