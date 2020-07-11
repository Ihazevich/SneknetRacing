using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Model
{
    public class MarshalZone : INotifyPropertyChanged
    {
        private float _zoneStart;   // Fraction (0..1) of way through the lap the marshal zone starts
        private byte _zoneFlag;    // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red

        public float ZoneStart
        {
            get
            {
                return _zoneStart;
            }
            set
            {
                _zoneStart = value;
                OnPropertyChanged("ZoneStart");
            }
        }

        public byte ZoneFlag
        {
            get
            {
                return _zoneFlag;
            }
            set
            {
                _zoneFlag = value;
                OnPropertyChanged("ZoneFlag");
            }
        }

        public void Desserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    ZoneStart = (float)reader.ReadDouble();
                    ZoneFlag = reader.ReadByte();
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
