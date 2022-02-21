using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Storages;
using System.Diagnostics;

namespace MainApplication.Services;

/// <summary>
///     Log Manager
/// </summary>
internal sealed class LogService
{
    private static readonly LogService Instance = new();
    private readonly ConfigurationService _configurationService = ConfigurationService.GetInstance();


    private readonly string localPath;
    private readonly string _logsFolderPath;
    private string? _logsPath;

    private AStorage<Log>? _storage;

    public LogService()
    {
        localPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"\Cesi-EasySave\");
        _logsFolderPath = Path.Join(localPath, @"data\logs\");
    }

    public List<LogFile> GetAllLogFiles()
    {
        Debug.WriteLine(_logsFolderPath);
        var listOfFiles = Directory.Exists(_logsFolderPath)
            ? Directory.GetFiles(_logsFolderPath)
            : Array.Empty<string>();

        return (from file in listOfFiles where _configurationService.Config.LogsFileType == FileType.XML && Path.GetExtension(file) == ".xml" || _configurationService.Config.LogsFileType == FileType.JSON && Path.GetExtension(file) == ".json" select new LogFile(Path.GetFileName(file), Path.GetFullPath(file))).ToList();
    }

    private void LoadLogsFile()
        {
            var dateTime = DateTime.Now;
            var date = dateTime.ToString("yyyy-MM-dd");
            if (_configurationService.Config.LogsFileType == FileType.XML)
            {
                _logsPath = _logsFolderPath + date + ".log.xml";
                _storage = new XmlStorage<Log>(_logsPath);
            }
            else
            {
                _logsPath = _logsFolderPath + date + ".log.json";
                _storage = new JsonStorage<Log>(_logsPath);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(_logsPath) ?? string.Empty);
            if (!File.Exists(_logsPath))
                File.CreateText(_logsPath).Close();
        }

        /// <summary>
        /// Insert log object into storage
        /// </summary>
        /// <param name="log"></param>
        public void InsertLog(Log log)
        {
            LoadLogsFile();
            _storage?.AddNewElementWithoutRewrite(log);
        }

        public static LogService GetInstance()
        {
            return Instance;
        }
    }