using MainApplication.Services.Sockets.Packets;
using System.Net.Sockets;
using System.Reflection;

namespace MainApplication.Services.Sockets;

//Todo Add IDisposable
public abstract class ASocket : IDisposable
{
    public List<PacketData> PacketsDataList;

    public Socket? Socket;

    public event EventHandler? Disposed;

    private bool closed;

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

    public object? InvokeMethodFromPacketmessage(PacketMessage packetMessage, Socket? socket)
    {

        var packetData = GetPacketDataByType(packetMessage);
        var receiveData = packetMessage?.Content;
        return packetData?.MethodInfo?.Invoke(packetData.Instance, new object?[] { receiveData, socket });

    }

    public void ShutdownSocket()
    {
        if (Socket == null)
            return;
        Socket.Shutdown(SocketShutdown.Both);
        Socket.Close();
    }


    #region IDISPOSABLE

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Socket?.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        if (closed)
            return;
        closed = true;
        Disposed?.Invoke(this, EventArgs.Empty);

        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion IDISPOSABLE
}
