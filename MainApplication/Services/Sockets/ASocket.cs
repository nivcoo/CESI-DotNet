using MainApplication.Services.Sockets.Packets;
using System.Net.Sockets;
using System.Reflection;

namespace MainApplication.Services.Sockets;

//Todo Add IDisposable
public abstract class ASocket
{
    public List<PacketData> PacketsDataList;

    public ASocket()
    {
        var methodsInfoList = Assembly.GetExecutingAssembly().GetTypes().SelectMany(x => x.GetMethods()).Where(m => m.GetCustomAttributes(typeof(PacketAttribute), false).Length > 0);
        PacketsDataList = new List<PacketData>();
        foreach (var methodInfo in methodsInfoList)
        {

            var attr = methodInfo.GetCustomAttribute(typeof(PacketAttribute)) as PacketAttribute;
            if (methodInfo == null || methodInfo.DeclaringType == null)
                continue;
            var instance = Activator.CreateInstance(methodInfo.DeclaringType);
            PacketsDataList.Add(new PacketData(instance, attr?.PacketType, methodInfo));
        }
    }

    public PacketData GetPacketDataByType(PacketMessage packetMessage)
    {
        return PacketsDataList.Single(packetData => packetData.PacketType == packetMessage.PacketType);
    }

    public string? InvokeMethodFromPacketmessage(PacketMessage packetMessage, Socket? socket)
    {

        var packetData = GetPacketDataByType(packetMessage);
        var receiveData = packetMessage?.Content;
        packetData?.MethodInfo?.Invoke(packetData.Instance, new object?[] { receiveData, socket });
        return receiveData;

    }
}
