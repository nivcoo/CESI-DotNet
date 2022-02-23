
using MainApplication.Handlers;
using MainApplication.Objects;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MainApplication.ViewModels.Client;

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

        var fileTempPath = Path.GetTempFileName();

        
        File.WriteAllText(fileTempPath, File.ReadAllText(logFile.FilePath));

        if (File.Exists(fileTempPath)) Process.Start("notepad.exe", fileTempPath);
    }
}
