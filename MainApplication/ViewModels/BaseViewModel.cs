using System.ComponentModel;
using System.Runtime.CompilerServices;
using MainApplication.Services;
using MainApplication.Annotations;

namespace MainApplication.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    internal readonly SaveService SaveService = SaveService.GetInstance();
    internal readonly LogService LogService = LogService.GetInstance();
    internal readonly ToolsService ToolsService = ToolsService.GetInstance();
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
        return ToolsService.ConvertStringIntegerToEnum<T>(choiceString);
    }
}