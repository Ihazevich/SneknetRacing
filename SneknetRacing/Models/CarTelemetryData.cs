using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Models
{
    public class CarTelemetryData : BaseModel
    {
        #region Fields
        private UInt16 _speed;                         // Speed of car in kilometres per hour
        private float _throttle;                      // Amount of throttle applied (0.0 to 1.0)
        private float _steer;                         // Steering (-1.0 (full lock left) to 1.0 (full lock right))
        private float _brake;                         // Amount of brake applied (0.0 to 1.0)
        private byte _clutch;                        // Amount of clutch applied (0 to 100)
        private sbyte _gear;                          // Gear selected (1-8, N=0, R=-1)
        private UInt16 _engineRPM;                     // Engine RPM
        private byte _drs;                           // 0 = off, 1 = on
        private byte _revLightsPercent;              // Rev lights indicator (percentage)
        private List<UInt16> _brakesTemperature = new List<UInt16>();          // Brakes temperature (celsius)
        private List<byte> _tyresSurfaceTemperature = new List<byte>();    // Tyres surface temperature (celsius)
        private List<byte> _tyresInnerTemperature = new List<byte>();      // Tyres inner temperature (celsius)
        private UInt16 _engineTemperature;             // Engine temperature (celsius)
        private List<float> _tyresPressure = new List<float>();              // Tyres pressure (PSI)
        private List<byte> _surfaceType = new List<byte>();                // Driving surface, see appendices
        #endregion

        #region Properties
        public UInt16 Speed
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
        public List<UInt16> BrakesTemperature
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
        public List<byte> TyresSurfaceTemperature
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
        public List<byte> TyresInnerTemperature
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
        public List<float> TyresPressure
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
        public List<byte> SurfaceType
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

        public override BaseModel Desserialize(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
