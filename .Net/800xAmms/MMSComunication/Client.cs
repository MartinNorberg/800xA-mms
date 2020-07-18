namespace MMSComunication
{
    using System;

    public class Client : IClient
    {
        private readonly string ipAddress;
        private readonly int port;

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
    }
}
