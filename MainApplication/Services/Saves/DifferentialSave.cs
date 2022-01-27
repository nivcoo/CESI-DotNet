using MainApplication.Annotations;
using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public class DifferentialASave : ASave
{
    public DifferentialASave(Save save) : base(save)
    {
    }

    public override bool RunSave()
    {
        return false;
    }
}