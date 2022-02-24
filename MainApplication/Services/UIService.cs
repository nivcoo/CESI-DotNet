using MainApplication.Services.Sockets.Chats;
using System.Diagnostics;
using System.Net;

namespace MainApplication.Services;

internal class UIService
{
    private static readonly UIService Instance = new();

    public Action<Action>? DispatchUiAction { get; set; }

    public bool IsServer = false;

    public ChatClient? ChatClient;

    public void InitSocket(bool isServer) {
        IsServer = isServer;
        if (IsServer)
        {

            try
            {
                var server = new ChatServer(IPAddress.Parse("127.0.0.1"), 55841);
                server.Start();

            }
            catch
            {
                IsServer = false; // if error go to client type
            }
        }


        if (!IsServer)
        {
            ChatClient = new ChatClient("127.0.0.1", 55841);

            ChatClient.ConnectAsync();
        }

    }


    public static UIService GetInstance()
    {
        return Instance;
    }
}