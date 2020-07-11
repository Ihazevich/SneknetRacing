using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Model
{
    public class WeatherForecastSample : INotifyPropertyChanged
    {
        private byte _sessionType;               // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P, 5 = Q1
                                                 // 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ, 10 = R, 11 = R2
                                                 // 12 = Time Trial
        private byte _timeOffset;                // Time in minutes the forecast is for
        private byte _weather;                   // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                                 // 3 = light rain, 4 = heavy rain, 5 = storm
        private sbyte _trackTemperature;         // Track temp. in degrees celsius
        private sbyte _airTemperature;           // Air temp. in degrees celsius

        public byte SessionType
        {
            get
            {
                return _sessionType;
            }
            set
            {
                _sessionType = value;
                OnPropertyChanged("SessionType");
            }
        }

        public byte TimeOffset
        {
            get
            {
                return _timeOffset;
            }
            set
            {
                _timeOffset = value;
                OnPropertyChanged("TimeOffset");
            }
        }

        public byte Weather
        {
            get
            {
                return _weather;
            }
            set
            {
                _weather = value;
                OnPropertyChanged("Weather");
            }
        }

        public sbyte TrackTemperature
        {
            get
            {
                return _trackTemperature;
            }
            set
            {
                _trackTemperature = value;
                OnPropertyChanged("TrackTemperature");
            }
        }

        public sbyte AirTemperature
        {
            get
            {
                return _airTemperature;
            }
            set
            {
                _airTemperature = value;
                OnPropertyChanged("AirTemperature");
            }
        }

        public void Desserialize(byte[] data)
        {
            PacketMotionData result = new PacketMotionData();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    SessionType = reader.ReadByte();
                    TimeOffset = reader.ReadByte();
                    Weather = reader.ReadByte();
                    TrackTemperature = reader.ReadSByte();
                    AirTemperature = reader.ReadSByte();
                }
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
