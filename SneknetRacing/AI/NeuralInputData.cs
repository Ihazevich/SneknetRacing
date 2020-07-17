using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;

namespace SneknetRacing.AI
{
    [Serializable]
    public class NeuralInputData : INotifyPropertyChanged
    {
        #region Fields
        private PacketCarSetupData _carSetupData = new PacketCarSetupData();
        private PacketCarStatusData _carStatusData = new PacketCarStatusData();
        private PacketCarTelemetryData _carTelemetryData = new PacketCarTelemetryData();
        //private PacketFinalClassificationData _finalClassificationData = new PacketFinalClassificationData();
        private PacketLapData _lapData = new PacketLapData();
        private PacketMotionData _motionData = new PacketMotionData();
        private PacketParticipantsData _participantsData = new PacketParticipantsData();
        #endregion

        #region Properties
        public PacketCarSetupData CarSetupData
        {
            get
            {
                return _carSetupData;
            }
            set
            {
                _carSetupData = value;
                OnPropertyChanged("CarSetupData");
            }
        }
        public PacketCarStatusData CarStatusData
        {
            get
            {
                return _carStatusData;
            }
            set
            {
                _carStatusData = value;
                OnPropertyChanged("CarStatusData");
            }
        }
        public PacketCarTelemetryData CarTelemetryData
        {
            get
            {
                return _carTelemetryData;
            }
            set
            {
                _carTelemetryData = value;
                OnPropertyChanged("CarTelemetryData");
            }
        }
        /*public PacketFinalClassificationData FinalClassificationData
        {
            get
            {
                return _finalClassificationData;
            }
            set
            {
                _finalClassificationData = value;
                OnPropertyChanged("FinalClassificationData");
            }
        }*/
        public PacketLapData LapData
        {
            get
            {
                return _lapData;
            }
            set
            {
                _lapData = value;
                OnPropertyChanged("LapData");
            }
        }
        public PacketMotionData MotionData
        {
            get
            {
                return _motionData;
            }
            set
            {
                _motionData = value;
                OnPropertyChanged("MotionData");
            }
        }
        public PacketParticipantsData ParticipantsData
        {
            get
            {
                return _participantsData;
            }
            set
            {
                _participantsData = value;
                OnPropertyChanged("MotionData");
            }
        }
        #endregion

        public NeuralInputData()
        {

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
