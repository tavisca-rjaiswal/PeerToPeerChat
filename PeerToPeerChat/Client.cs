using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PeerToPeerChat
{
    class Client
    {
        Socket _sender;
        IPEndPoint _localEndPoint;

        public Client(string serverIP, int serverPortToConnect)
        {
            if (IPAddress.TryParse(serverIP, out IPAddress serverIPAddress))
                _localEndPoint = new IPEndPoint(serverIPAddress, serverPortToConnect);
            else
                throw new Exception("Invalid IP to connect");

            _sender = new Socket(serverIPAddress.AddressFamily,
                       SocketType.Stream, ProtocolType.Tcp);
        }

        public Client(IPAddress serverIP, int serverPortToConnect)
        {
            _localEndPoint = new IPEndPoint(serverIP, serverPortToConnect);

            _sender = new Socket(serverIP.AddressFamily,
                       SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            try
            {
                try
                {
                    _sender.Connect(_localEndPoint);

                    Console.WriteLine("Socket connected to -> {0} ",
                                  _sender.RemoteEndPoint.ToString());
                    while (true)
                    {
                        string message = Console.ReadLine();
                        if (message == "exit") break;
                        byte[] messageSent = Encoding.ASCII.GetBytes(message);
                        int byteSent = _sender.Send(messageSent);
                        byte[] messageReceived = new byte[1024];
                        int byteRecv = _sender.Receive(messageReceived);
                        Console.WriteLine("Message from Server -> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
                    }
                    _sender.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
