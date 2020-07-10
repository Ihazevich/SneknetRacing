using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SneknetRacing.Network
{
    public class Server
    {
        public void Listen()
        {
            UdpClient listener = new UdpClient();
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20777);

            while(true)
            {
                byte[] data = listener.Receive(ref serverEP);
                RaiseDataReceived(new ReceivedDataArgs(serverEP.Address, serverEP.Port, data));
            }
        }

        public delegate void DataReceived(object sender, ReceivedDataArgs args);

        public event DataReceived DataReceivedEvent;

        public void RaiseDataReceived(ReceivedDataArgs args)
        {
            DataReceivedEvent?.Invoke(this, args);
        }
    }

    public class ReceivedDataArgs
    {
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }
        public byte[] receivedBytes;

        public ReceivedDataArgs(IPAddress ip, int port, byte[] data)
        {
            IpAddress = ip;
            Port = port;
            receivedBytes = data;
        }
    }
}
