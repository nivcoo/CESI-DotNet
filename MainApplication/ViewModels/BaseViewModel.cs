
using MainApplication.Services;
using MainApplication.Objects;
using MainApplication.Handlers;

namespace MainApplication.ViewModels;
/// <summary>
/// The base and generic viewmodel for the application 
/// </summary>
public class BaseViewModel : INPChanged
{
    internal readonly SaveService SaveService = SaveService.GetInstance();

    public static T? ConvertStringIntegerToEnum<T>(string? choiceString)
    {
        return ToolService.ConvertStringIntegerToEnum<T>(choiceString);
    }

    public Save? AlreadySaveWithSameName(string? saveName)
    {
        return SaveService.AlreadySaveWithSameName(saveName);
    }
}