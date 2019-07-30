using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PeerToPeerChat
{
    class Server
    {
        private int _port;
        Socket _listener;
        IPEndPoint _localEndPoint;

        public Server(int port)
        {
            _port = port;
            IPHostEntry ipHost = Dns.GetHostEntryAsync(Dns.GetHostName()).GetAwaiter().GetResult();
            IPAddress ipAddress = ipHost.AddressList
                    .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
            _localEndPoint = new IPEndPoint(ipAddress, _port);

            _listener = new Socket(ipAddress.AddressFamily,
                 SocketType.Stream, ProtocolType.Tcp);
        }

        public void Listen()
        {
            try
            {
                _listener.Bind(_localEndPoint);
                Console.WriteLine($"Server started - {_localEndPoint}");
                _listener.Listen(10);
                Console.WriteLine("Waiting connection ... ");
                Socket clientSocket = _listener.Accept();

                while (true)
                {
                    byte[] bytes = new Byte[1024];
                    string data = null;
                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes,0, numByte);
                        Console.WriteLine("Text received -> {0} ", data);
                        string message = Console.ReadLine();
                        if (message == "exit") break;
                        byte[] messageSent = Encoding.ASCII.GetBytes(message);
                        clientSocket.Send(messageSent);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                _listener.Shutdown(SocketShutdown.Both);
            }
        }
    }
}
