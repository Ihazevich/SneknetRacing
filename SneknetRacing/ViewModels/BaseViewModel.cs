using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private int _totalPackets = 0;
        private BaseModel _packet;
        private ConcurrentQueue<byte[]> _receivedRawPackets = new ConcurrentQueue<byte[]>();
        private ConcurrentQueue<BaseModel> _processedPackets = new ConcurrentQueue<BaseModel>();

        public int TotalPackets
        {
            get
            {
                return _totalPackets;
            }
            set
            {
                _totalPackets = value;
                OnPropertyChanged("TotalPackets");
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
            DesserializationThread = Task.Factory.StartNew(() => Desserialize());
        }

        public BaseViewModel(BaseModel packet) : this()
        {
            Packet = packet;
        }

        public virtual void Desserialize()
        {
            while(true)
            {
                byte[] rawPacket;
                if (ReceivedPackets.TryDequeue(out rawPacket))
                {
                    Packet = Packet.Desserialize(rawPacket);
                    ProcessedPackets.Enqueue(Packet);
                    TotalPackets--;
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
