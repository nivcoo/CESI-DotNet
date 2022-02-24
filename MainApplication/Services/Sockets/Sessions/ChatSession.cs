using MainApplication.Services.Sockets.Packets;
using NetCoreServer;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace MainApplication.Services.Sockets.Sessions;

public class ChatSession : TcpSession
{
    public List<PacketData> PacketsDataList;

    public ChatSession(TcpServer server) : base(server) {

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

    protected override void OnConnected()
    {
        Console.WriteLine($"Chat TCP session with Id {Id} connected!");
    }

    protected override void OnDisconnected()
    {
        Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
    }

    public object? InvokeMethodFromPacketmessage(PacketMessage packetMessage, ChatSession? session)
    {

        var packetData = GetPacketDataByType(packetMessage);
        var receiveData = packetMessage?.Content;
        return packetData?.MethodInfo?.Invoke(packetData.Instance, new object?[] { receiveData, session });

    }

    public void SendPacketToClient(PacketType packetType, object? obj)
    {
        SendAsync(Encoding.UTF8.GetBytes(ToolService.SerializeObject(new PacketMessage(packetType, ToolService.SerializeObject(obj))) + "<EOF><EOFN>"));
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        var raw = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);

        foreach (var packet in raw.Replace("<EOF>", string.Empty)
                     .Split("<EOFN>")
                     .Where(x => x != ""))
        {
            var packetMessage = ToolService.DeserializeObject<PacketMessage>(packet);
            if (packetMessage != null)
            {
                 InvokeMethodFromPacketmessage(packetMessage, this);
            }
        }

        // Multicast message to all connected sessions
        //Server.Multicast(message);

        // If the buffer starts with '!' the disconnect the current session
        /*if (message == "!")
            Disconnect();*/
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Chat TCP session caught an error with code {error}");
    }
}
