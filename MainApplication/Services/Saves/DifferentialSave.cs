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
        throw new NotImplementedException();
    }
}