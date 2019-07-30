using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PeerToPeerChat
{
    class Program
    {
        static void Main(string[] args)
        {
            string ch = Console.ReadLine();
            Console.WriteLine(ch);

            try
            {
                switch (ch.ToLower())
                {
                    case "s":
                        Server server = new Server(9090);
                        server.Listen();
                        break;

                    case "c":
                        IPHostEntry ipHost = Dns.GetHostEntryAsync(Dns.GetHostName()).GetAwaiter().GetResult();
                        IPAddress ipAddress = ipHost.AddressList
                                .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);

                        Client client = new Client(ipAddress, 9090);
                        client.Connect();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
