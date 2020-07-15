using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SneknetRacing.Network
{
    public class Server : INotifyPropertyChanged
    {
        private long _totalPackets = 0;
        private static double _avgPacketsPerSecond = 0;
        private Stopwatch _stopwatch;

        public double AvgPacketsPerSecond
        {
            get
            {
                return _avgPacketsPerSecond;
            }
            set
            {
                _avgPacketsPerSecond = value;
                OnPropertyChanged("AvgPacketsPerSecond");
            }
        }

        public Server()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void Listen()
        {
            UdpClient listener = new UdpClient(20777);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20777);

            while(true)
            {
                byte[] data = listener.Receive(ref serverEP);
                _totalPackets++;
                if(_stopwatch.ElapsedMilliseconds % 1000 == 0)
                {
                    AvgPacketsPerSecond = _totalPackets * 1000 / _stopwatch.ElapsedMilliseconds;
                }
                RaiseDataReceived(new ReceivedDataArgs(serverEP.Address, serverEP.Port, data));
            }
        }

        public delegate void DataReceived(object sender, ReceivedDataArgs args);

        public event DataReceived DataReceivedEvent;

        public void RaiseDataReceived(ReceivedDataArgs args)
        {
            DataReceivedEvent?.Invoke(this, args);
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

}
