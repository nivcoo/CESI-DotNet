using System.Reflection;

namespace MainApplication.Services.Sockets.Packets;

public class PacketData
{
    public object? Instance;
    public PacketType? PacketType;
    public MethodInfo? MethodInfo;

    public PacketData(object? instance, PacketType? packetType, MethodInfo? methodInfo)
    {
        Instance = instance;
        PacketType = packetType;
        MethodInfo = methodInfo;
    }
}
