using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.Network
{
    class HandleDataClass
    {
        public void SubscribeToEvent(Server server)
        {
            server.DataReceivedEvent += server_DataReceivedEvent;
        }

        private void server_DataReceivedEvent(object sender, ReceivedDataArgs args)
        {
            Console.WriteLine("Received message: \r\n{2}", Encoding.ASCII.GetString(args.receivedBytes));
        }
    }
}
