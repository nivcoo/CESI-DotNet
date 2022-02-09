using MainApplication.Handlers;
using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Services;
using System.Collections.ObjectModel;

namespace MainApplication.ViewModels;

public class SavesViewModel : BaseViewModel
{


    public ObservableCollection<Save> Saves { get; }

    private CommandHandler? _startSaveButtonEvent;

    public CommandHandler StartSaveButtonEvent
    {
        get { return _startSaveButtonEvent ??= _startSaveButtonEvent = new CommandHandler(StartSaveEvent, CanStartSave); }
    }

    private CommandHandler? _pauseSaveButtonEvent;

    public CommandHandler PauseSaveButtonEvent
    {
        get { return _pauseSaveButtonEvent ??= _pauseSaveButtonEvent = new CommandHandler(PauseSaveEvent); }
    }

    private CommandHandler? _deleteSaveButtonEvent;

    public CommandHandler DeleteSaveButtonEvent
    {
        get { return _deleteSaveButtonEvent ??= _deleteSaveButtonEvent = new CommandHandler(DeleteSaveEvent); }
    }

    private CommandHandler? _resumeSaveButtonEvent;

    public CommandHandler ResumeSaveButtonEvent
    {
        get { return _resumeSaveButtonEvent ??= _resumeSaveButtonEvent = new CommandHandler(ResumeSaveEvent, CanStartSave); }
    }


    private CommandHandler? _startAllSavesEvent;

    public CommandHandler StartAllSavesEvent
    {
        get { return _startAllSavesEvent ??= _startAllSavesEvent = new CommandHandler(StartAllSaves, CanStartSave); }
    }

    private CommandHandler? _resumeAllSavesEvent;

    public CommandHandler ResumeAllSavesEvent
    {
        get { return _resumeAllSavesEvent ??= _resumeAllSavesEvent = new CommandHandler(ResumeAllSaves, CanStartSave); }
    }

    private CommandHandler? _pauseAllSavesEvent;

    public CommandHandler PauseAllSavesEvent
    {
        get { return _pauseAllSavesEvent ??= _pauseAllSavesEvent = new CommandHandler(PauseAllSaves); }
    }

    private Action<string>? _savePageErrorAction;

    public Action<string>? SavePageErrorAction
    {
        get => _savePageErrorAction;
        set => SetField(ref _savePageErrorAction, value, nameof(SavePageErrorAction));
    }

    private Action<Action>? _dispatchUiAction;

    public Action<Action>? DispatchUiAction { 
        get => _dispatchUiAction; 
        set { 
            _dispatchUiAction = value; 
            if(value != null)
                EasySaveService.DispatchUiAction = value; 
        }
    }

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

    private void StartAllSaves(object? args)
    {
        SaveService.StartAllSaves();
    }

    private void PauseAllSaves(object? args)
    {
        SaveService.PauseAllSaves();
    }

    private void ResumeAllSaves(object? args)
    {
        SaveService.ResumeAllSaves();
    }

    private void StartSaveEvent(object? args)
    {
        if (args is not Save save)
            return;
        SaveService.StartSave(save);

    }

    private void ResumeSaveEvent(object? args)
    {
        if (args is not Save save)
            return;
        SaveService.SetStateOfSave(save, false);

    }

    private void PauseSaveEvent(object? args)
    {
        if (args is not Save save)
            return;
        SaveService.SetStateOfSave(save, true);

    }

    private void DeleteSaveEvent(object? args)
    {
        if (args is not Save save)
            return;

        SaveService.RemoveSave(save);
        Saves.Remove(save);
    }

    private bool CanStartSave(object? origin)
    {
        if (origin is not CommandHandler ch)
            return true;
        bool canStart = !EasySaveService.JobApplicationIsRunning(ch);
        string message = string.Empty;
        if (!canStart)
            message = Language.ERROR_JOB_APPLICATION_RUNNING;
        SendSaveError(message);
        return canStart;
    }

    public void SendSaveError(string message) {
        SavePageErrorAction?.Invoke(message);
    }
}