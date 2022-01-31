using System.ComponentModel;
using System.Runtime.CompilerServices;
using MainApplication.Services;
using MainApplication.Annotations;
using MainApplication.Objects;

namespace MainApplication.ViewModels;
/// <summary>
/// The base and generic viewmodel for the application 
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    internal readonly SaveService SaveService = SaveService.GetInstance();
    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, string propertyName)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public static T? ConvertStringIntegerToEnum<T>(string? choiceString)
    {
        return ToolService.ConvertStringIntegerToEnum<T>(choiceString);
    }

    public Save? AlreadySaveWithSameName(string? saveName)
    {
        return SaveService.AlreadySaveWithSameName(saveName);
    }
}