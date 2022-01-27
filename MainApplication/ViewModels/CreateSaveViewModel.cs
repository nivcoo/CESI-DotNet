using MainApplication.Objects;
using MainApplication.Services;

namespace MainApplication.ViewModels;

public class CreateSaveViewModel : BaseViewModel
{

    public bool AddNewSave(Save save)
    {
        return SaveService.AddNewSave(save);
    }

    public static Uri? IsValidUri(string? uri)
    {
        return ToolService.IsValidUri(uri);
    }
}