namespace MainApplication.Services.Sockets.Packets;

public class PacketMessage
{

    public PacketType PacketType { get; set; }

    public string Content { get; set; }

    public PacketMessage(PacketType packetType, string content)
    {
        PacketType = packetType;
        Content = content;
    }

}
