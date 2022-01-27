using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public abstract class ASave
{
    protected Save Save { get; set; }

    protected ASave(Save save)
    {
        Save = save;
    }

    public abstract bool RunSave();
}