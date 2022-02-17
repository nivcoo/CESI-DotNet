using MainApplication.Handlers;
using MainApplication.Objects;
using MainApplication.Services;

namespace MainApplication.ViewModels;

/// <summary>
///     The base and generic viewmodel for the application
/// </summary>
public class BaseViewModel : INPChanged
{
    internal readonly ConfigurationService ConfigurationService = ConfigurationService.GetInstance();
    internal readonly EasySaveService EasySaveService = EasySaveService.GetInstance();
    internal readonly SaveService SaveService = SaveService.GetInstance();
    internal readonly LogService LogService = LogService.GetInstance();

    public static T? ConvertStringIntegerToEnum<T>(string? choiceString)
    {
        return ToolService.ConvertStringIntegerToEnum<T>(choiceString);
    }

    public Save? AlreadySaveWithSameName(string? saveName)
    {
        return SaveService.AlreadySaveWithSameName(saveName);
    }
}