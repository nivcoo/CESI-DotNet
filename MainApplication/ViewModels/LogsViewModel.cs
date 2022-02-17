
using MainApplication.Handlers;
using MainApplication.Objects;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MainApplication.ViewModels;

public class LogsViewModel : BaseViewModel
{

    public ObservableCollection<LogFile> LogFiles { get; }

    private CommandHandler? _openLogButtonEvent;

    public CommandHandler OpenLogButtonEvent
    {
        get
        {
            return _openLogButtonEvent ??= _openLogButtonEvent = new CommandHandler(StartSaveEvent);
        }
    }

    public LogsViewModel()
    {

        LogFiles = new ObservableCollection<LogFile>();
        foreach (var logFile in LogService.GetAllLogFiles()) {
            LogFiles.Add(logFile);
        }
    }


    private void StartSaveEvent(object? args)
    {
        if (args is not LogFile logFile)
            return;

        if (File.Exists(logFile.FilePath)) Process.Start("notepad.exe", logFile.FilePath);
    }
}