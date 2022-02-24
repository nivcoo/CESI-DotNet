using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services.Sockets;
using MainApplication.Services.Sockets.Packets;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;

namespace MainApplication.ViewModels.Home;

public class ClientHomeViewModel : AHomeViewModel
{
    private Config _config;
    private readonly ClientSocket? _clientSocket;


    public ClientHomeViewModel() : base()
    {
        _config = ConfigurationService.Config;


        _clientSocket = ConfigurationService.ClientSocket;
        SetRemoteConfig();

        UpdateEncryptExtensionsList();

        UpdatePriorityFilesList();

        UpdateStats();
    }

    public Task<object?>? SendPacket(PacketType packetType, object? obj)
    {
        return _clientSocket?.SendPacket(packetType, obj, false);
    }

    public Task<object?>? SendPacket(PacketType packetType, object? obj, bool waitResult)
    {
        return _clientSocket?.SendPacket(packetType, obj, waitResult);
    }

    public void SetRemoteConfig()
    {

        if (SendPacket(PacketType.Server_HomePage_GetConfig, null, true)?.Result is Config remoteConfig)
            _config = remoteConfig;
    }

    public void ChangeMaxFileSize(double maxFileSize)
    {

        SendPacket(PacketType.Server_HomePage_ChangeMaxFileSize, maxFileSize);
        SetRemoteConfig();
    }

    public void ChangeSavesFileType(FileType fileType)
    {

        SendPacket(PacketType.Server_HomePage_ChangeSavesFileType, fileType);
        SetRemoteConfig();
    }

    public void ChangeLogsFileType(FileType fileType)
    {

        SendPacket(PacketType.Server_HomePage_ChangeSavesFileType, fileType);
        SetRemoteConfig();
    }

    public override double MaxFileSize
    {
        get => _config.MaxFileSize;
        set => ChangeMaxFileSize(value);
    }


    public override FileType SelectedSavesFileType
    {
        get => _config.SavesFileType;
        set => ChangeSavesFileType(value);
    }

    public override CultureInfo SelectedCultureInfo
    {
        get => EasySaveService.SelectedCultureInfo;
        set => EasySaveService.ChangeCulture(value);
    }

    public override List<CultureInfo> AllCultureInfo
    {
        get => EasySaveService.AllCultureInfo;
        set => SetField(ref EasySaveService.AllCultureInfo, value, nameof(AllCultureInfo));
    }

    public override FileType SelectedLogsFileType
    {
        get => _config.LogsFileType;
        set => ChangeLogsFileType(value);
    }

    public override void UpdateStats()
    {
        StatSavesNumber = SaveService.GetSaves().Count;
        StatLogsNumber = LogService.GetAllLogFiles().Count;
    }

    public override void RemoveEncryptExtensionEvent(object? args)
    {
        if (args is not string extension)
            return;

        ConfigurationService.RemoveEncryptExtension(extension);
        EncryptExtensions.Remove(extension);
    }


    public override void RemovePriorityFileEvent(object? args)
    {
        if (args is not string priorityFile)
            return;

        ConfigurationService.RemovePriorityFile(priorityFile);
        PriorityFiles.Remove(priorityFile);
    }

    public override void UpdateEncryptExtensionsList()
    {
        EncryptExtensions.Clear();
        foreach (var extensionName in _config.EncryptExtensions)
            EncryptExtensions.Add(extensionName);
    }

    public override void UpdatePriorityFilesList()
    {
        PriorityFiles.Clear();
        foreach (var fileName in _config.PriorityFiles)
            PriorityFiles.Add(fileName);
    }


}

