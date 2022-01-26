using MainApplication.Objects;
using MainApplication.Services;

namespace MainApplication.ViewModels;

public class CreateSaveViewModel : BaseViewModel
{
    private List<Save>? _saves;
    public List<Save>? Saves
    {
        get => _saves;
        set => SetField(ref _saves, value, nameof(Saves));
    }
    
    private Save? _save;
    public Save? Save
    {
        get => _save;
        set => SetField(ref _save, value, nameof(Save));
    }
}