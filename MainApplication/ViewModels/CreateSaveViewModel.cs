using MainApplication.Objects;
using MainApplication.Services;

namespace MainApplication.ViewModels;

public class CreateSaveViewModel : BaseViewModel
{
    
    /// <summary>
    /// Add save with save object
    /// </summary>
    /// <param name="save"></param>
    /// <returns>true if Success</returns>

    public bool AddNewSave(Save save)
    {
        return SaveService.AddNewSave(save);
    }

    /// <summary>
    /// Is value Uri
    /// </summary>
    /// <param name="uri"></param>
    /// <returns>uri object or null if error</returns>
    public static Uri? IsValidUri(string? uri)
    {
        return ToolService.IsValidUri(uri);
    }
}