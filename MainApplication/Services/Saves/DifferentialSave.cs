using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public class DifferentialSave : ISave
{
    public Save Save { get; set; }

    public DifferentialSave(Save save)
    {
        Save = save;
    }

    public bool RunSave()
    {
        throw new NotImplementedException();
    }
}