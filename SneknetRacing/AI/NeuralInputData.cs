using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.AI
{
    public class NeuralInputData : INotifyPropertyChanged
    {
        private BindingList<PacketCarStatusData> _carStatusDataPackets;
        private BindingList<PacketCarTelemetryData> _carTelemetryDataPackets;
        private BindingList<PacketLapData> _lapDataPackets;
        private BindingList<PacketMotionData> _motionDataPackets;

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
            _carStatusDataPackets.AllowRemove = true;
            _carStatusDataPackets.RaiseListChangedEvents = true;

            _carTelemetryDataPackets.AllowNew = true;
            _carTelemetryDataPackets.AllowEdit = false;
            _carTelemetryDataPackets.AllowRemove = true;
            _carTelemetryDataPackets.RaiseListChangedEvents = true;

            _lapDataPackets.AllowNew = true;
            _lapDataPackets.AllowEdit = false;
            _lapDataPackets.AllowRemove = true;
            _lapDataPackets.RaiseListChangedEvents = true;

            _motionDataPackets.AllowNew = true;
            _motionDataPackets.AllowEdit = false;
            _motionDataPackets.AllowRemove = true;
            _motionDataPackets.RaiseListChangedEvents = true;

        }

        private void ListChanged(object sender, ListChangedEventArgs e)
        {
            if(MotionDataPackets.Count != 0 && LapDataPackets.Count != 0 && CarTelemetryDataPackets.Count != 0 && CarStatusDataPackets.Count != 0)
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
