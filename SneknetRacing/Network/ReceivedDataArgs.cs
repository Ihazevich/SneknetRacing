using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SneknetRacing.Network
{
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
