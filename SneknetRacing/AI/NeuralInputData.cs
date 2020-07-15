using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;

namespace SneknetRacing.AI
{
    public class NeuralInputData : INotifyPropertyChanged
    {
        private BindingList<PacketCarStatusData> _carStatusDataPackets;
        private BindingList<PacketCarTelemetryData> _carTelemetryDataPackets;
        private BindingList<PacketLapData> _lapDataPackets;
        private BindingList<PacketMotionData> _motionDataPackets;

        private object _carStatusDataPacketsLock = new object();
        private object _carTelemetryDataPacketsLock = new object();
        private object _lapDataPacketsLock = new object();
        private object _motionDataPacketsLock = new object();

        public BindingList<PacketCarStatusData> CarStatusDataPackets
        {
            get
            {
                return _carStatusDataPackets;
            }
            set
            {
                _carStatusDataPackets = value;
                OnPropertyChanged("CarStatusDataPackets");
            }
        }
        public BindingList<PacketCarTelemetryData> CarTelemetryDataPackets
        {
            get
            {
                return _carTelemetryDataPackets;
            }
            set
            {
                _carTelemetryDataPackets = value;
                OnPropertyChanged("CarTelemetryDataPackets");
            }
        }
        public BindingList<PacketLapData> LapDataPackets
        {
            get
            {
                return _lapDataPackets;
            }
            set
            {
                _lapDataPackets = value;
                OnPropertyChanged("LapDataPackets");
            }
        }
        public BindingList<PacketMotionData> MotionDataPackets
        {
            get
            {
                return _motionDataPackets;
            }
            set
            {
                _motionDataPackets = value;
                OnPropertyChanged("MotionDataPackets");
            }
        }

        public NeuralInputData()
        {

            _carStatusDataPackets = new BindingList<PacketCarStatusData>();
            _carTelemetryDataPackets = new BindingList<PacketCarTelemetryData>();
            _lapDataPackets = new BindingList<PacketLapData>();
            _motionDataPackets = new BindingList<PacketMotionData>();

            _carStatusDataPackets.AllowNew = true;
            _carStatusDataPackets.AllowEdit = false;

            _carTelemetryDataPackets.AllowNew = true;
            _carTelemetryDataPackets.AllowEdit = false;

            _lapDataPackets.AllowNew = true;
            _lapDataPackets.AllowEdit = false;

            _motionDataPackets.AllowNew = true;
            _motionDataPackets.AllowEdit = false;

            BindingOperations.EnableCollectionSynchronization(CarStatusDataPackets, _carStatusDataPackets);
            BindingOperations.EnableCollectionSynchronization(CarTelemetryDataPackets, _carTelemetryDataPackets);
            BindingOperations.EnableCollectionSynchronization(LapDataPackets, _lapDataPackets);
            BindingOperations.EnableCollectionSynchronization(MotionDataPackets, _motionDataPackets);

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
