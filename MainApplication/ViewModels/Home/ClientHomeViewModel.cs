using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services.Sockets;
using MainApplication.Services.Sockets.Chats;
using MainApplication.Services.Sockets.Packets;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;

namespace MainApplication.ViewModels.Home;

public class ClientHomeViewModel : AHomeViewModel
{
    private readonly ChatClient? _chatClient;


    public ClientHomeViewModel() : base()
    {

        _chatClient = UIService.ChatClient;
        SetRemoteConfig();

        UpdateEncryptExtensionsList();

        UpdatePriorityFilesList();

        UpdateStats();

        ConfigurationService.Config.RegisterToEvent(NotifyEasySaveServicePropertyChanged);
    }

    private void NotifyEasySaveServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ConfigurationService.Config.Language):
                SetCultureInfo();
                OnPropertyChanged(nameof(SelectedCultureInfo));
                OnPropertyChanged(nameof(AllCultureInfo));
                break;
        }
        
    }

    public override List<CultureInfo> AllCultureInfo
    {
        get => EasySaveService.AllCultureInfo;
        set => SetField(ref EasySaveService.AllCultureInfo, value, nameof(AllCultureInfo));
    }

    public void SendPacket(PacketType packetType, object? obj)
    {
        _chatClient?.SendPacket(packetType, obj);
    }

    public void SetRemoteConfig()
    {
        SendPacket(PacketType.Server_HomePage_GetConfig, null);
    }

    public void SetCultureInfo()
    {
        EasySaveService.SelectedCultureInfo = CultureInfo.GetCultureInfo(ConfigurationService.Config.Language);
        Language.Culture = EasySaveService.SelectedCultureInfo;
        UpdateLocalizationAction?.Invoke();
        UpdateComboBoxAction?.Invoke();
    }

    public void ChangeMaxFileSize(double maxFileSize)
    {

        SendPacket(PacketType.Server_HomePage_ChangeMaxFileSize, maxFileSize);
        ConfigurationService.Config.MaxFileSize = maxFileSize;
    }

    public void ChangeSavesFileType(FileType fileType)
    {

        SendPacket(PacketType.Server_HomePage_ChangeSavesFileType, fileType);
        ConfigurationService.Config.SavesFileType = fileType;
    }

    public void ChangeLogsFileType(FileType fileType)
    {

        SendPacket(PacketType.Server_HomePage_ChangeLogsFileType, fileType);
        ConfigurationService.Config.LogsFileType = fileType;
    }

    public void ChangeCulture(CultureInfo culture)
    {
        if (culture == EasySaveService.SelectedCultureInfo)
            return;
        SendPacket(PacketType.Server_HomePage_ChangeCulture, culture.Name);
        EasySaveService.ChangeCulture(culture);
        ConfigurationService.Config.Language = culture.Name;
    }

    public override double MaxFileSize
    {
        get => ConfigurationService.Config.MaxFileSize;
        set => ChangeMaxFileSize(value);
    }


    public override FileType SelectedSavesFileType
    {
        get => ConfigurationService.Config.SavesFileType;
        set => ChangeSavesFileType(value);
    }

    public override CultureInfo SelectedCultureInfo
    {
        get => EasySaveService.SelectedCultureInfo;
        set => ChangeCulture(value);
    }

    public override FileType SelectedLogsFileType
    {
        get => ConfigurationService.Config.LogsFileType;
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
        foreach (var extensionName in ConfigurationService.Config.EncryptExtensions)
            EncryptExtensions.Add(extensionName);
    }

    public override void UpdatePriorityFilesList()
    {
        PriorityFiles.Clear();
        foreach (var fileName in ConfigurationService.Config.PriorityFiles)
            PriorityFiles.Add(fileName);
    }


}

