using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SneknetRacing.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private long _totalPackets = 0;
        private BaseModel _packet;
        private ConcurrentQueue<byte[]> _receivedRawPackets = new ConcurrentQueue<byte[]>();
        private ConcurrentQueue<BaseModel> _processedPackets = new ConcurrentQueue<BaseModel>();

        private Stopwatch _stopwatch;

        public long TotalPackets
        {
            get
            {
                return _totalPackets;
            }
            set
            {
                _totalPackets = value;
                OnPropertyChanged("TotalPackets");
                OnPropertyChanged("PacketsPerSecond");
            }
        }

        public string PacketsPerSecond
        {
            get 
            {
                return "Packets: " + TotalPackets + " | Ellapsed time in seconds: " + _stopwatch.ElapsedMilliseconds / 1000 + " | Rate: " + TotalPackets / (_stopwatch.ElapsedMilliseconds / 1000) + " packets/sec";
            }
        }

        public BaseModel Packet
        {
            get
            {
                return _packet;
            }
            set
            {
                _packet = value;
                OnPropertyChanged("Packet");
            }
        }

        public ConcurrentQueue<byte[]> ReceivedPackets
        {
            get
            {
                return _receivedRawPackets;
            }
            set
            {
                _receivedRawPackets = value;
                OnPropertyChanged("ReceivedPackets");
            }
        }

        public ConcurrentQueue<BaseModel> ProcessedPackets
        {
            get
            {
                return _processedPackets;
            }
            set
            {
                _processedPackets = value;
                OnPropertyChanged("ProcessedPackets");
            }
        }

        public Task DesserializationThread { get; }
        
        public BaseViewModel()
        {
            _stopwatch = Stopwatch.StartNew(); 
            DesserializationThread = Task.Factory.StartNew(() => Desserialize());
        }

        public virtual void Desserialize()
        {
            Console.WriteLine(this + " DesserializationThread started...");
            while(true)
            {
                byte[] rawPacket;
                if (ReceivedPackets.TryDequeue(out rawPacket))
                {
                    Packet = Packet.Desserialize(rawPacket);
                    ProcessedPackets.Enqueue(Packet);
                }
            }
        }

        public virtual bool AddPacketToDesserializationQueue(byte[] data)
        {
            try
            {
                ReceivedPackets.Enqueue(data);
                TotalPackets++;
                return true;
            }
            catch(Exception)
            {
                return false;
                throw;
            }
            finally
            {
            }
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
