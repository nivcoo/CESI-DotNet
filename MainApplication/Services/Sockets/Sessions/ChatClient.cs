using MainApplication.Services.Sockets.Packets;
using NetCoreServer;
using System.Reflection;
using System.Text;

namespace MainApplication.Services.Sockets.Chats;

public class ChatClient : TcpClient
{


    public List<PacketData> PacketsDataList;

    public ChatClient(string address, int port) : base(address, port)


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

    public void DisconnectAndStop()
    {
        _stop = true;
        DisconnectAsync();
        while (IsConnected)
            Thread.Yield();
    }

    protected override void OnConnected()
    {
        Console.WriteLine($"Chat TCP client connected a new session with Id {Id}");
    }

    protected override void OnDisconnected()
    {
        Console.WriteLine($"Chat TCP client disconnected a session with Id {Id}");

        // Wait for a while...
        Thread.Sleep(1000);

        // Try to connect again
        if (!_stop)
            ConnectAsync();
    }

    public object? InvokeMethodFromPacketmessage(PacketMessage packetMessage, ChatClient sessionClient)
    {

        var packetData = GetPacketDataByType(packetMessage);
        var receiveData = packetMessage?.Content;
        return packetData?.MethodInfo?.Invoke(packetData.Instance, new object?[] { receiveData, sessionClient });

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
    }

    protected override void OnError(System.Net.Sockets.SocketError error)
    {
        Console.WriteLine($"Chat TCP client caught an error with code {error}");
    }


    public void SendPacket(PacketType packetType, object? obj)
    {
        SendAsync(Encoding.UTF8.GetBytes(ToolService.SerializeObject(new PacketMessage(packetType, ToolService.SerializeObject(obj))) + "<EOF><EOFN>"));
    }

    private bool _stop;
}
