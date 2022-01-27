using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public interface ISave
{
    Save Save { get; set; }
    
    bool RunSave();
}