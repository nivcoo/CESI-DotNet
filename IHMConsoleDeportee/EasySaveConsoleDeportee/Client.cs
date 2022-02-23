using EasySaveConsoleDeportee.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsoleDeportee
{
    public class Client
    {
        byte[] buffer = new byte[1024];
        IPEndPoint server;
        Socket socket;

        public void SeConnecter()
        {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint server = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55894);
        socket.Connect(server);                    
        }

        public string EcouterReseau(string message)
        {
            socket.Send(Serializer.Serialize(message));
            int test = socket.Receive(buffer);
            string retourValeur = Serializer.Deserialize(buffer, test);
            return retourValeur;
        }

        public string RequeteListeSave()
        {
            string message = "Liste";
            string retourValeur = EcouterReseau(message);
            return retourValeur;
        }

        public string RequeteLancer()
        {
            string message = "Lancement";
            string retourValeur = EcouterReseau(message);
            return retourValeur;
        }

        public string RequeteStopper()
        {
            string message = "Pause";
            string retourValeur = EcouterReseau(message);
            return retourValeur;
        }

        public string RequeteReprendre()
        {
            string message = "Reprise";
            string retourValeur = EcouterReseau(message);
            return retourValeur;
        }

        public string RequeteArreter()
        {
            string message = "Arret";
            string retourValeur = EcouterReseau(message);
            return retourValeur;
        }

        public void Deconnecter() 
        { 
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}