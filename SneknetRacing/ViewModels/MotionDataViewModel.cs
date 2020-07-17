using SneknetRacing.Models;
using SneknetRacing.Views;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.ViewModels
{
    public class MotionDataViewModel : BaseViewModel
    {
        private PacketMotionData _packet = new PacketMotionData();
        private CarMotionData _selectedCarMotionData = new CarMotionData();
        private ConcurrentQueue<byte[]> _receivedRawPackets = new ConcurrentQueue<byte[]>();
        private ConcurrentQueue<PacketMotionData> _processedPackets = new ConcurrentQueue<PacketMotionData>();

        public PacketMotionData Packet
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

        public ConcurrentQueue<PacketMotionData> ProcessedPackets
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
        public SelectedCarMotionDataView SelectedCarMotionDataView { get; }

        public CarMotionData SelectedCarMotionData
        {
            get
            {
                return _selectedCarMotionData;
            }
            set
            {
                _selectedCarMotionData = value;
                OnPropertyChanged("SelectedCarMotionData");
            }
        }

        public MotionDataViewModel()
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
                    Packet = Packet.Desserialize(rawPacket) as PacketMotionData;
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
