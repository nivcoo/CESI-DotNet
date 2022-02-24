using MainApplication.Objects;
using MainApplication.Services.Sockets.Packets;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MainApplication.Services.Sockets;

//Todo Add IDisposable
public class ClientSocket : ASocket
{
    public ClientSocket() : base()
    {
        ConnectClient();
    }

    public void ConnectClient()
    {

        try
        {
            IPEndPoint remoteEP = new(IPAddress.Parse("127.0.0.1"), 55584);

            Socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Socket.Connect(remoteEP);

            }
            catch { }

        }
        catch { }
    }

    public Task<object?>? SendPacket(PacketType packetType, object? obj)
    {
        return SendPacket(packetType, obj, false);
    }

    public Task<object?>? SendPacket(PacketType packetType, object? obj, bool waitResult)
    {
        if (Socket == null)
            return null;
        byte[] bytes = new byte[1024];
        Task<object?> sendPacketTask = Task.Run(() => {
            try
            {
                Socket.Send(Encoding.UTF8.GetBytes(ToolService.SerializeObject(new PacketMessage(packetType, ToolService.SerializeObject(obj))) + "<EOF>"), SocketFlags.None);

                if (waitResult)
                {
                    string data = "";
                    int bytesRec;
                    while ((bytesRec = Socket.Receive(bytes)) > 0)
                    {
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    data = data.Replace("<EOF>", "");

                    
                    var packetMessage = ToolService.DeserializeObject<PacketMessage>(data);
                    if (packetMessage != null)
                    {
                        return InvokeMethodFromPacketmessage(packetMessage, null);
                    }
                    return null;
                }
                
                
            }
            catch { }

            return null;
        });

        return sendPacketTask;
    }

}
