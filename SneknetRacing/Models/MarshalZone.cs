using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class MarshalZone : BaseModel
    {
        private float _zoneStart;   // Fraction (0..1) of way through the lap the marshal zone starts
        private sbyte _zoneFlag;    // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red

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

        public sbyte ZoneFlag
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

        public override BaseModel Desserialize(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
