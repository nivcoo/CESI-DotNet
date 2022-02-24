using MainApplication.Services.Sockets.Packets;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace MainApplication.Services.Sockets;

// todo idisposable
public class ServerSocket : ASocket
{

    public List<Socket> ClientsSockets;

    public ServerSocket() : base()
    {
        ClientsSockets = new List<Socket>();

        Task.Run(() => StartListening());
    }

    public void StartListening()
    {
        byte[] bytes = new byte[1024];

        IPEndPoint localEndPoint = new (IPAddress.Parse("127.0.0.1"), 55584);
        Socket = new (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            Socket.Bind(localEndPoint);
            Socket.Listen(10);

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                Socket clientSocket = Socket.Accept();
                Task.Run(() =>
                {
                    ClientsSockets.Add(clientSocket);
                    string otherPacket = "";
                    while (clientSocket.Connected)
                    {
                        string data = otherPacket;
                        int bytesRec;
                        
                        while ((bytesRec = clientSocket.Receive(bytes)) > 0)
                        {
                            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            if (data.IndexOf("<EOF>") > -1)
                            {
                                string[] dataArray = data.Split(new[] { "<EOF>" }, StringSplitOptions.None);
                                otherPacket = dataArray[1];
                                data = dataArray[0] + "<EOF>";
                                break;
                            }
                        }
                        otherPacket = "";
                        if (data == null)
                            return;
                        data = data.Replace("<EOF>", "");
                        var packetMessage = ToolService.DeserializeObject<PacketMessage>(data);
                        if (packetMessage != null)
                        {
                            var receiveData = InvokeMethodFromPacketmessage(packetMessage, clientSocket);
                        }
                    }
                    ClientsSockets.Remove(clientSocket);
                });


                
            }

        }
        catch { }

    }
}

