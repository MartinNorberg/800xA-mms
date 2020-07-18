namespace MMSComunication
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class Client : IClient, IDisposable
    {
        private readonly string ipAddress;
        private readonly int port;
        private Socket client;
        private bool disposed;

        public Client(string ipAddress, int port = 102)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new ArgumentException("IpAddress is empty");
            }

            this.ipAddress = ipAddress;
            this.port = port;
        }

        public int Port => port;

        public string IpAddress => ipAddress;

        public void StartClient()
        {
            try
            {
                var iPAddress = IPAddress.Parse(this.ipAddress);
                var remoteEndPoint = new IPEndPoint(iPAddress, port);
                this.client?.Dispose();
                this.client = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.client.Connect(remoteEndPoint);
                this.Receive();
            }
            catch (Exception e)
            {
                Thread.Sleep(1500);
                this.StartClient();
            }
        }

        private void Receive()
        {
            try
            {
                while (true)
                {
                    var buffer = new byte[256];
                    var bytesRead = this.client.Receive(buffer, buffer.Length, SocketFlags.None);

                    if (bytesRead > 0)
                    {
                        var newMessage = new NewMessageEventArgs();
                        newMessage.Message = bytesRead.ToString();                                                
                        this.OnNewMessage(newMessage);
                    }
                }
            }
            catch (Exception e)
            {
                Thread.Sleep(1500);
                // Never stop... unless we want to.
                this.StartClient();
            }
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.client?.Dispose();
        }
        
        protected virtual void OnNewMessage(NewMessageEventArgs e)
        {
            EventHandler<NewMessageEventArgs> handler = this.NewMessage;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<NewMessageEventArgs> NewMessage;
        // ska vi skicka in en lista med taggar? som uppdateras? eller när "new message" kommer så kan man köra en "getNewData" så retunerar vi en uppdaterad lista?

    }
}
