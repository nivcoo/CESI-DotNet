using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services.Sockets.Packets;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace MainApplication.Services.Sockets.Packet;

public class PacketMethod
{
    internal static readonly ConfigurationService ConfigurationService = ConfigurationService.GetInstance();
    internal static readonly EasySaveService EasySaveService = EasySaveService.GetInstance();
    internal static readonly SaveService SaveService = SaveService.GetInstance();
    internal static readonly LogService LogService = LogService.GetInstance();
    public static void SendPacket(Socket socket, PacketType packetType, object? obj)
    {
        try
        {
            socket.Send(Encoding.UTF8.GetBytes(ToolService.SerializeObject(new PacketMessage(packetType, ToolService.SerializeObject(obj))) + "<EOF>"), SocketFlags.None);
        }
        catch { }

    }


    [Packet(PacketType.Server_HomePage_GetConfig)]
    public static void Server_HomePage_GetConfig(string serializedObject, Socket socket)
    {
        SendPacket(socket, PacketType.Client_HomePage_ReturnConfig, ConfigurationService.Config);
    }

    [Packet(PacketType.Server_HomePage_ChangeMaxFileSize)]
    public static void Server_HomePage_ChangeMaxFileSize(string serializedObject, Socket socket)
    {
        ConfigurationService.ChangeMaxFileSize(double.Parse(serializedObject));
    }

    [Packet(PacketType.Server_HomePage_ChangeSavesFileType)]
    public static void Server_HomePage_ChangeSavesFileType(string serializedObject, Socket socket)
    {
        var fileType = ToolService.DeserializeObject<FileType>(serializedObject);
        ConfigurationService.ChangeSavesFileType(fileType);
    }

    [Packet(PacketType.Server_HomePage_ChangeLogsFileType)]
    public static void Server_HomePage_ChangeLogsFileType(string serializedObject, Socket socket)
    {
        var fileType = ToolService.DeserializeObject<FileType>(serializedObject);
        ConfigurationService.ChangeLogsFileType(fileType);
    }






    [Packet(PacketType.Client_HomePage_ReturnConfig)]
    public static object? Client_HomePage_ReturnConfig(string serializedObject, Socket? socket)
    {
        var config = ToolService.DeserializeObject<Config>(serializedObject);
        if (config == null)
            return null;
        return config;
    }

}
