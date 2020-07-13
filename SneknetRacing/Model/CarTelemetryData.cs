using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Model
{
    public class CarTelemetryData : BaseModel
    {
        #region Fields
        private byte _speed;                         // Speed of car in kilometres per hour
        private float _throttle;                      // Amount of throttle applied (0.0 to 1.0)
        private float _steer;                         // Steering (-1.0 (full lock left) to 1.0 (full lock right))
        private float _brake;                         // Amount of brake applied (0.0 to 1.0)
        private byte _clutch;                        // Amount of clutch applied (0 to 100)
        private sbyte _gear;                          // Gear selected (1-8, N=0, R=-1)
        private UInt16 _engineRPM;                     // Engine RPM
        private byte _drs;                           // 0 = off, 1 = on
        private byte _revLightsPercent;              // Rev lights indicator (percentage)
        private UInt16[] _brakesTemperature;          // Brakes temperature (celsius)
        private byte[] _tyresSurfaceTemperature;    // Tyres surface temperature (celsius)
        private byte[] _tyresInnerTemperature;      // Tyres inner temperature (celsius)
        private UInt16 _engineTemperature;             // Engine temperature (celsius)
        private float[] _tyresPressure;              // Tyres pressure (PSI)
        private byte[] _surfaceType;                // Driving surface, see appendices
        #endregion

        #region Properties
        public byte Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                OnPropertyChanged("Speed");
            }
        }
        public float Throttle
        {
            get
            {
                return _throttle;
            }
            set
            {
                _throttle = value;
                OnPropertyChanged("Throttle");
            }
        }
        public float Steer
        {
            get
            {
                return _steer;
            }
            set
            {
                _steer = value;
                OnPropertyChanged("Steer");
            }
        }
        public float Brake
        {
            get
            {
                return _brake;
            }
            set
            {
                _brake = value;
                OnPropertyChanged("Brake");
            }
        }
        public byte Clutch
        {
            get
            {
                return _clutch;
            }
            set
            {
                _clutch = value;
                OnPropertyChanged("Clutch");
            }
        }
        public sbyte Gear
        {
            get
            {
                return _gear;
            }
            set
            {
                _gear = value;
                OnPropertyChanged("Gear");
            }
        }
        public UInt16 EngineRPM
        {
            get
            {
                return _engineRPM;
            }
            set
            {
                _engineRPM = value;
                OnPropertyChanged("EngineRPM");
            }
        }
        public byte Drs
        {
            get
            {
                return _drs;
            }
            set
            {
                _drs = value;
                OnPropertyChanged("Drs");
            }
        }
        public byte RevLightsPercent
        {
            get
            {
                return _revLightsPercent;
            }
            set
            {
                _revLightsPercent = value;
                OnPropertyChanged("RevLightsPercent");
            }
        }
        public UInt16[] BrakesTemperature
        {
            get
            {
                return _brakesTemperature;
            }
            set
            {
                _brakesTemperature = value;
                OnPropertyChanged("BrakesTemperature");
            }
        }
        public byte[] TyresSurfaceTemperature
        {
            get
            {
                return _tyresSurfaceTemperature;
            }
            set
            {
                _tyresSurfaceTemperature = value;
                OnPropertyChanged("TyresSurfaceTemperature");
            }
        }
        public byte[] TyresInnerTemperature
        {
            get
            {
                return _tyresInnerTemperature;
            }
            set
            {
                _tyresInnerTemperature = value;
                OnPropertyChanged("tyresInnerTemperature");
            }
        }
        public UInt16 EngineTemperature
        {
            get
            {
                return _engineTemperature;
            }
            set
            {
                _engineTemperature = value;
                OnPropertyChanged("EngineTemperature");
            }
        }
        public float[] TyresPressure
        {
            get
            {
                return _tyresPressure;
            }
            set
            {
                _tyresPressure = value;
                OnPropertyChanged("TyresPressure");
            }
        }
        public byte[] SurfaceType
        {
            get
            {
                return _surfaceType;
            }
            set
            {
                _surfaceType = value;
                OnPropertyChanged("SurfaceType");
            }
        }
        #endregion
    }
}
