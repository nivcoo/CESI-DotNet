using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services;

namespace MainApplication.ViewModels.Client;

public class CreateSaveViewModel : BaseViewModel
{
    private TypeSave[] _savesType = (TypeSave[]) Enum.GetValues(typeof(TypeSave));

    public TypeSave[] SavesType
    {
        get => _savesType;
        set => SetField(ref _savesType, value, nameof(TypeSave));
    }

    /// <summary>
    ///     Add save with save object
    /// </summary>
    /// <param name="save"></param>
    /// <returns>true if Success</returns>
    public bool AddNewSave(Save save)
    {
        return SaveService.AddNewSave(save);
    }

    /// <summary>
    ///     Is value Uri
    /// </summary>
    /// <param name="uri"></param>
    /// <returns>uri object or null if error</returns>
    public static Uri? IsValidUri(string? uri)
    {
        return ToolService.IsValidUri(uri);
    }
}