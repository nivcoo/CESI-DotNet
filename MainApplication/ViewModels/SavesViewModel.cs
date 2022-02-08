using MainApplication.Handlers;
using MainApplication.Objects;
using MainApplication.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace MainApplication.ViewModels;

public class SavesViewModel : BaseViewModel
{


    public ObservableCollection<Save> Saves { get; }

    private ICommand? _startSaveButtonEvent;

    public ICommand StartSaveButtonEvent
    {
        get { return _startSaveButtonEvent ??= _startSaveButtonEvent = new CommandHandler(StartSaveEvent); }
    }

    private ICommand? _deleteSaveButtonEvent;

    public ICommand DeleteSaveButtonEvent
    {
        get { return _deleteSaveButtonEvent ??= _deleteSaveButtonEvent = new CommandHandler(DeleteSaveEvent); }
    }

    public Action<Action> DispatchUiAction { get; set; }

    public SavesViewModel()
    {
        Saves = new ObservableCollection<Save>();
        UpdateSavesList();
    }

    public void UpdateSavesList()
    {
        Saves.Clear();
        foreach (var save in SaveService.GetSaves())
            Saves.Add(save);
    }

    private void StartSaveEvent(object? args)
    {
        if (args is not Save save)
            return;
        SaveService.StartSave(save, DispatchUiAction);
        
    }

    private void DeleteSaveEvent(object? args)
    {
        if (args is not Save save)
            return;

        SaveService.RemoveSave(save);
        Saves.Remove(save);
    }
}