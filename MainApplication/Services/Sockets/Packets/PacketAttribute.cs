namespace MainApplication.Services.Sockets.Packets;

[AttributeUsage(AttributeTargets.Method)]
public class PacketAttribute : Attribute
{
    public PacketType PacketType;

    public PacketAttribute(PacketType packetType)
    {
        PacketType = packetType;
    }
}
