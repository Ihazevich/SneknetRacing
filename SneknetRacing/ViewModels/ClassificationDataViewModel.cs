using SneknetRacing.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.ViewModels
{
    public class ClassificationDataViewModel : BaseViewModel
    {
        private PacketFinalClassificationData _packet = new PacketFinalClassificationData();
        private ConcurrentQueue<byte[]> _receivedRawPackets = new ConcurrentQueue<byte[]>();
        private ConcurrentQueue<PacketFinalClassificationData> _processedPackets = new ConcurrentQueue<PacketFinalClassificationData>();

        public PacketFinalClassificationData Packet
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

        public ConcurrentQueue<PacketFinalClassificationData> ProcessedPackets
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

        public ClassificationDataViewModel()
        {
            DesserializationThread = Task.Factory.StartNew(() => Desserialize());
        }

        public void Desserialize()
        {
            Console.WriteLine(this + " DesserializationThread started...");
            while (true)
            {
                byte[] rawPacket;
                if (ReceivedPackets.TryDequeue(out rawPacket))
                {
                    Packet = Packet.Desserialize(rawPacket) as PacketFinalClassificationData;
                    ProcessedPackets.Enqueue(Packet);
                }
            }
        }

        public bool AddPacketToDesserializationQueue(byte[] data)
        {
            try
            {
                ReceivedPackets.Enqueue(data);
                TotalPackets++;
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally
            {
            }
        }
    }
}
