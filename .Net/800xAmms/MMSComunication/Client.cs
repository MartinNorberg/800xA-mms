namespace MMSComunication
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class Client : IClient, IDisposable
    {
        private readonly string ipAddress;
        private readonly int port;
        private bool isStarted;
        private TcpListener server;
        private TcpClient client;
        private Socket socket;
        NetworkStream stream;
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
                this.server = new TcpListener(IPAddress.Parse(this.ipAddress), this.port);
                this.server.Start();
                this.isStarted = true;
                this.client?.Dispose();
                this.client = server.AcceptTcpClient();
                
                if (this.InitilazeMmsCommunication())
                {
                    this.Receive();
                }
            }
            catch (Exception e)
            {
                Thread.Sleep(1500);
                this.StartClient();
            }
        }

        private bool InitilazeMmsCommunication()
        {
            var bytes = new Byte[1024];
            var data = string.Empty;

            try
            {
                this.stream?.Dispose();
                this.stream = client.GetStream();


                var i = stream.Read(bytes, 0, bytes.Length);
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                bytes[5] = 208;
                bytes[6] = bytes[8];
                bytes[7] = bytes[9];
                bytes[8] = 1;
                bytes[9] = 56;
                bytes[13] = 2;
                bytes[17] = 1;

                this.Send(bytes, stream);
                i = stream.Read(bytes, 0, bytes.Length);
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                this.Send(new byte[] { 3, 0, 0, 46, 2, 240, 128, 169, 37, 128, 2, 3, 251, 129, 1, 3, 130, 1, 3, 131, 1, 127, 164, 22, 128, 1, 1, 129, 3, 5, 232, 0, 130, 12, 3, 236, 0, 120, 63, 15, 244, 16, 3, 1, 0, 144 }, stream);

                return true;
            }
            catch (Exception)
            {
                return false;                
            }
           
        }

        private void Receive()
        {
            try
            {
                var bytes = new Byte[1024];
                var data = string.Empty;


                while (this.isStarted)
                {
                    var i = stream.Read(bytes, 0, bytes.Length);
                    if (MMSVariable.TryGetMmsVariables(bytes, out var result))
                    {
                        var receivedMmsData = new List<MMSVariable>(result.Count);

                        foreach (var variable in result)
                        {
                            receivedMmsData.Add(variable);
                        }

                        var newMessage = new NewMessageEventArgs();
                        newMessage.MmsVariables = receivedMmsData;
                        this.OnNewMessage(newMessage);
                    }
                    
                    
                }

                client.Close();
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public void Send(byte[] sendData, NetworkStream stream)
        {
            var writeLength = sendData[3] | (sendData[2] << 8) | (sendData[1] << 16);
            stream.Write(sendData, 0, writeLength);
        }

        public virtual void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.stream?.Dispose();
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
    }
}
