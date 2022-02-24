using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services.Sockets.Chats;
using MainApplication.Services.Sockets.Packets;
using MainApplication.Services.Sockets.Sessions;
using System.Globalization;

namespace MainApplication.Services.Sockets.Packet;

public class PacketMethod
{
    internal static readonly ConfigurationService ConfigurationService = ConfigurationService.GetInstance();
    internal static readonly EasySaveService EasySaveService = EasySaveService.GetInstance();
    internal static readonly SaveService SaveService = SaveService.GetInstance();
    internal static readonly LogService LogService = LogService.GetInstance();


    [Packet(PacketType.Server_HomePage_GetConfig)]
    public static void Server_HomePage_GetConfig(string serializedObject, ChatSession session)
    {
        session.SendPacketToClient(PacketType.Client_HomePage_SetConfig, ConfigurationService.Config);
    }

    [Packet(PacketType.Server_HomePage_ChangeMaxFileSize)]
    public static void Server_HomePage_ChangeMaxFileSize(string serializedObject, ChatSession session)
    {
        ConfigurationService.ChangeMaxFileSize(double.Parse(serializedObject));
    }

    [Packet(PacketType.Server_HomePage_ChangeSavesFileType)]
    public static void Server_HomePage_ChangeSavesFileType(string serializedObject, ChatSession session)
    {
        var fileType = ToolService.DeserializeObject<FileType>(serializedObject);
        ConfigurationService.ChangeSavesFileType(fileType);
    }

    [Packet(PacketType.Server_HomePage_ChangeLogsFileType)]
    public static void Server_HomePage_ChangeLogsFileType(string serializedObject, ChatSession session)
    {
        var fileType = ToolService.DeserializeObject<FileType>(serializedObject);
        ConfigurationService.ChangeLogsFileType(fileType);
    }


    [Packet(PacketType.Server_HomePage_ChangeCulture)]
    public static void Server_HomePage_ChangeCulture(string serializedObject, ChatSession session)
    {

        var cultureString = ToolService.DeserializeObject<string>(serializedObject);
        if (cultureString == null)
            return;
        EasySaveService.ChangeCulture(CultureInfo.GetCultureInfo(cultureString));

    }



    [Packet(PacketType.Client_HomePage_SetConfig)]
    public static void Client_HomePage_SetConfig(string serializedObject, ChatClient? sessionClient)
    {
        var config = ToolService.DeserializeObject<Config>(serializedObject);
        if (config == null)
            return;
        ConfigurationService.Config.Language = config.Language;
        ConfigurationService.Config.MaxFileSize = config.MaxFileSize;
        ConfigurationService.Config.SavesFileType = config.SavesFileType;
        ConfigurationService.Config.LogsFileType = config.LogsFileType;
    }


}
