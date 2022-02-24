using MainApplication.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MainApplication.Handlers;

public abstract class INPChanged : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {

        ExecuteActionOnUIThread(() =>
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        });
        
    }

    protected bool SetField<T>(ref T field, T value, string propertyName)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void ExecuteActionOnUIThread(Action action)
    {
        var uiThread = UIService.GetInstance().DispatchUiAction;

        if (uiThread == null)
            action.Invoke();
        else
            uiThread.Invoke(action.Invoke);
    }


    public void RegisterToEvent(PropertyChangedEventHandler propertyChanged)
    {
        PropertyChanged -= propertyChanged;
        PropertyChanged += propertyChanged;
    }
    
}