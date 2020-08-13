namespace Sandbox
{
    using MMSComunication;
    using Sandbox;
    using System;
    class Program
    {
        static void Main(string[] args)
        {       
            var client = new Client("127.0.0.1", 102);
            client.NewMessage += NewMessage;
            client.StartClient();
        }

        static void NewMessage(object sender, NewMessageEventArgs e)
        {
            Console.WriteLine("Incomming!!!!");
            foreach (var item in e.MmsVariables)
            {                
                Console.WriteLine(item.ToString());
            }
        }
    }
}
