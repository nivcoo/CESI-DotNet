using MainApplication.Objects;
using MainApplication.Services.Sockets.Packets;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MainApplication.Services.Sockets;

//Todo Add IDisposable
public class Client : ASocket
{
    public Socket? ClientSocket;
    public Client()
    {
        ConnectClient();
        SendPacket(PacketType.Server_HomePageGetConfig, new Log("rtrt", new Uri(@"C:\"), new Uri(@"C:\"), 0, 0, 0, DateTime.Now));
        SendPacket(PacketType.Server_HomePageGetConfig, new Log("rtrt", new Uri(@"C:\"), new Uri(@"C:\"), 0, 0, 0, DateTime.Now));
    }

    public void ConnectClient()
    {

        try
        {
            IPEndPoint remoteEP = new(IPAddress.Parse("127.0.0.1"), 55584);

            ClientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ClientSocket.Connect(remoteEP);

            }
            catch { }

        }
        catch { }
    }
    public void ShutdownSocket()
    {
        if (ClientSocket == null)
            return;
        ClientSocket.Shutdown(SocketShutdown.Both);
        ClientSocket.Close();
    }

    public void SendPacket(PacketType packetType, object obj)
    {
        if (ClientSocket == null)
            return;
        byte[] bytes = new byte[1024];
        Task.Run(() => {
            try
            {
                ClientSocket.Send(Encoding.UTF8.GetBytes(ToolService.SerializeObject(new PacketMessage(packetType, ToolService.SerializeObject(obj))) + "<EOF>"));


                string? data = null;
                while (true)
                {
                    int bytesRec = ClientSocket.Receive(bytes);
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
                    InvokeMethodFromPacketmessage(packetMessage, null);
                }
                
            }
            catch { }
        });
    }

}
