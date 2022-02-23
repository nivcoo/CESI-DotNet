using MainApplication.Objects;
using MainApplication.Services.Sockets.Packets;
using System.Net.Sockets;
using System.Text;

namespace MainApplication.Services.Sockets.Packet;

public class PacketMethod
{
    public static void SendPacket(Socket socket, PacketType packetType, object obj)
    {
        try
        {
            socket.Send(Encoding.UTF8.GetBytes(ToolService.SerializeObject(new PacketMessage(packetType, ToolService.SerializeObject(obj))) + "<EOF>"));
        }
        catch { }

    }

    [Packet(PacketType.Server_HomePageGetConfig)]
    public static void HomePageGetConfig(string serializedObject, Socket socket)
    {
        var log = ToolService.DeserializeObject<Log>(serializedObject);
        SendPacket(socket, PacketType.Client_HomePageGet, log);
    }

    [Packet(PacketType.Client_HomePageGet)]
    public static void HomePageGet(string serializedObject, Socket? socket)
    {
        Console.WriteLine("rhrthrt");
    }

}
