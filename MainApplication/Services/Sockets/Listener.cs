using MainApplication.Services.Sockets.Packets;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace MainApplication.Services.Sockets;

// todo idisposable
public class Listener : ASocket
{


    public Socket? ServerSocket;

    public List<Socket> ClientsSockets;

    public Listener()
    {
        ClientsSockets = new List<Socket>();

        Task.Run(() => StartListening());
    }

    public void StartListening()
    {
        byte[] bytes = new byte[1024];

        IPEndPoint localEndPoint = new (IPAddress.Parse("127.0.0.1"), 55584);
        ServerSocket = new (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            ServerSocket.Bind(localEndPoint);
            ServerSocket.Listen(10);

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                Socket clientSocket = ServerSocket.Accept();
                Task.Run(() =>
                {
                    ClientsSockets.Add(clientSocket);
                    while (clientSocket.Connected)
                    {
                        string? data = null;
                        while (true)
                        {
                            int bytesRec = clientSocket.Receive(bytes);
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
                            var receiveData = InvokeMethodFromPacketmessage(packetMessage, clientSocket);
                            Console.WriteLine("Text received : {0}", receiveData);
                        }
                    }
                    ClientsSockets.Remove(clientSocket);
                });


                
            }

        }
        catch { }

    }

    public void ShutdownSocket()
    {
        if (ServerSocket == null)
            return;
        ServerSocket.Shutdown(SocketShutdown.Both);
        ServerSocket.Close();
    }
}

