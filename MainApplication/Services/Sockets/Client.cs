using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MainApplication.Services.Sockets
{
    public class Client : IDisposable
    {
        private bool closed;
        private readonly Socket socket;

        public event EventHandler Disposed;

        public Client(Socket socket)
        {
            this.socket = socket;
        }

        public void Start()
        {
            socket.Send(Serializer.Serialize("Welcome to the server"));
            Thread thread = new Thread(Listen);
            thread.Start();
        }

        private void Listen()
        {
            byte[] buffer = new byte[1024];

            while (!closed)
            {
                try
                {
                    int count = socket.Receive(buffer);
                    SaveFunction(buffer, count);
                    if (count == 0)
                        throw new SocketException();
                }
                catch (SocketException)
                {
                    Dispose();
                }
            }
        }

        private void SaveFunction(byte[] bytes, int value)
        {
            string reception = Serializer.Deserialize(bytes, value);

            string[] fonctionEtNom = reception.Split('/');

            switch (fonctionEtNom[0])
            {
                case "Lancement":

                case "Pause":

                case "Reprise":

                case "Arret":

                case "Liste":

                default: break;
            }
        }

        #region IDISPOSABLE

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    socket.Dispose();
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
}