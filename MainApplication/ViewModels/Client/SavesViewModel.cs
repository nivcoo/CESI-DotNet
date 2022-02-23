using System.Collections.ObjectModel;
using MainApplication.Handlers;
using MainApplication.Localization;
using MainApplication.Objects;

namespace MainApplication.ViewModels.Client;

public class SavesViewModel : BaseViewModel
{
    private CommandHandler? _deleteSaveButtonEvent;

    private Action<Action>? _dispatchUiAction;

    private CommandHandler? _pauseAllSavesEvent;

    private CommandHandler? _pauseSaveButtonEvent;

    private CommandHandler? _resumeAllSavesEvent;

    private CommandHandler? _resumeSaveButtonEvent;

    private Action<string>? _savePageErrorAction;


    private CommandHandler? _startAllSavesEvent;

    private CommandHandler? _startSaveButtonEvent;



    public ObservableCollection<Save> Saves { get => SaveService.GetSaves(); }

    public CommandHandler StartSaveButtonEvent
    {
        get
        {
            return _startSaveButtonEvent ??= _startSaveButtonEvent = new CommandHandler(StartSaveEvent, CanStartSave);
        }
    }

    public CommandHandler PauseSaveButtonEvent
    {
        get { return _pauseSaveButtonEvent ??= _pauseSaveButtonEvent = new CommandHandler(PauseSaveEvent); }
    }

    public CommandHandler DeleteSaveButtonEvent
    {
        get { return _deleteSaveButtonEvent ??= _deleteSaveButtonEvent = new CommandHandler(DeleteSaveEvent); }
    }

    public CommandHandler ResumeSaveButtonEvent
    {
        get
        {
            return _resumeSaveButtonEvent ??=
                _resumeSaveButtonEvent = new CommandHandler(ResumeSaveEvent, CanStartSave);
        }
    }

    public CommandHandler StartAllSavesEvent
    {
        get { return _startAllSavesEvent ??= _startAllSavesEvent = new CommandHandler(StartAllSaves, CanStartSave); }
    }

    public CommandHandler ResumeAllSavesEvent
    {
        get { return _resumeAllSavesEvent ??= _resumeAllSavesEvent = new CommandHandler(ResumeAllSaves, CanStartSave); }
    }

    public CommandHandler PauseAllSavesEvent
    {
        get { return _pauseAllSavesEvent ??= _pauseAllSavesEvent = new CommandHandler(PauseAllSaves); }
    }

    public Action<string>? SavePageErrorAction
    {
        get => _savePageErrorAction;
        set => SetField(ref _savePageErrorAction, value, nameof(SavePageErrorAction));
    }

    public Action<Action>? DispatchUiAction
    {
        get => _dispatchUiAction;
        set
        {
            _dispatchUiAction = value;
            if (value != null)
                EasySaveService.DispatchUiAction = value;
        }
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
    }

    private bool CanStartSave(object? origin)
    {
        if (origin is not CommandHandler ch)
            return true;
        var canStart = !EasySaveService.JobApplicationIsRunning(ch.RaiseCanExecuteChanged);
        var message = string.Empty;
        if (!canStart)
            message = Language.ERROR_JOB_APPLICATION_RUNNING;
        SendSaveError(message);
        return canStart;
    }

    public void SendSaveError(string message)
    {
        SavePageErrorAction?.Invoke(message);
    }
}
