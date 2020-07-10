using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Model
{
    public struct PacketMotionDataStruct
    {
        PacketHeader header;                  // Header

        CarMotionData[] carMotionData;        // Data for all cars on track

        // Extra player car ONLY data
        float[] suspensionPosition;           // Note: All wheel arrays have the following order:
        float[] suspensionVelocity;           // RL, RR, FL, FR
        float[] suspensionAcceleration;       // RL, RR, FL, FR
        float[] wheelSpeed;                   // Speed of each wheel
        float[] wheelSlip;                    // Slip ratio for each wheel
        float localVelocityX;                 // Velocity in local space
        float localVelocityY;                 // Velocity in local space
        float localVelocityZ;                 // Velocity in local space
        float angularVelocityX;               // Angular velocity x-component
        float angularVelocityY;               // Angular velocity y-component
        float angularVelocityZ;               // Angular velocity z-component
        float angularAccelerationX;           // Angular velocity x-component
        float angularAccelerationY;           // Angular velocity y-component
        float angularAccelerationZ;           // Angular velocity z-component
        float frontWheelsAngle;               // Current front wheels angle in radians
    }

    public class PacketMotionData : INotifyPropertyChanged
    {
        private PacketHeader _header;                  // Header

        private CarMotionData[] _carMotionData;        // Data for all cars on track

        // Extra player car ONLY data
        private float[] _suspensionPosition;           // Note: All wheel arrays have the following order:
        private float[] _suspensionVelocity;           // RL, RR, FL, FR
        private float[] _suspensionAcceleration;       // RL, RR, FL, FR
        private float[] _wheelSpeed;                   // Speed of each wheel
        private float[] _wheelSlip;                    // Slip ratio for each wheel
        private float _localVelocityX;                 // Velocity in local space
        private float _localVelocityY;                 // Velocity in local space
        private float _localVelocityZ;                 // Velocity in local space
        private float _angularVelocityX;               // Angular velocity x-component
        private float _angularVelocityY;               // Angular velocity y-component
        private float _angularVelocityZ;               // Angular velocity z-component
        private float _angularAccelerationX;           // Angular velocity x-component
        private float _angularAccelerationY;           // Angular velocity y-component
        private float _angularAccelerationZ;           // Angular velocity z-component
        private float _frontWheelsAngle;               // Current front wheels angle in radians

        public PacketHeader Header
        {
            get 
            {
                return _header;
            }
            set
            {
                _header = value;
                OnPropertyChanged("Header");
            }
        }

        public CarMotionData[] CarMotionData
        {
            get
            {
                return _carMotionData;
            }
            set
            {
                _carMotionData = value;
                OnPropertyChanged("CarMotionData");
            }
        }
        public float[] SuspensionPosition
        {
            get
            {
                return _suspensionPosition;
            }
            set
            {
                _suspensionPosition = value;
                OnPropertyChanged("SuspensionPosition");
            }
        }

        public float[] SuspensionVelocity
        {
            get
            {
                return _suspensionVelocity;
            }
            set
            {
                _suspensionVelocity = value;
                OnPropertyChanged("SuspensionVelocity");
            }
        }

        public float[] SuspensionAcceleration
        {
            get
            {
                return _suspensionAcceleration;
            }
            set
            {
                _suspensionAcceleration = value;
                OnPropertyChanged("SuspensionAcceleration");
            }
        }

        public float[] WheelSpeed
        {
            get
            {
                return _wheelSpeed;
            }
            set
            {
                _wheelSpeed = value;
                OnPropertyChanged("WheelSpeed");
            }
        }

        public float[] WheelSlip
        {
            get
            {
                return _wheelSlip;
            }
            set
            {
                _wheelSlip = value;
                OnPropertyChanged("WheelSlip");
            }
        }

        public float LocalVelocityX
        {
            get
            {
                return _localVelocityX;
            }
            set
            {
                _localVelocityX = value;
                OnPropertyChanged("LocalVelocityX");
            }
        }

        public float LocalVelocityY
        {
            get
            {
                return _localVelocityY;
            }
            set
            {
                _localVelocityY = value;
                OnPropertyChanged("LocalVelocityY");
            }
        }

        public float LocalVelocityZ
        {
            get
            {
                return _localVelocityZ;
            }
            set
            {
                _localVelocityZ = value;
                OnPropertyChanged("LocalVelocityZ");
            }
        }

        public float AngularVelocityX
        {
            get
            {
                return _angularVelocityX;
            }
            set
            {
                _angularVelocityX = value;
                OnPropertyChanged("AngularVelocityX");
            }
        }

        public float AngularVelocityY
        {
            get
            {
                return _angularVelocityY;
            }
            set
            {
                _angularVelocityY = value;
                OnPropertyChanged("AngularVelocityY");
            }
        }

        public float AngularVelocityZ
        {
            get
            {
                return _angularVelocityZ;
            }
            set
            {
                _angularVelocityZ = value;
                OnPropertyChanged("AngularVelocityZ");
            }
        }

        public float AngularAccelerationX
        {
            get
            {
                return _angularAccelerationX;
            }
            set
            {
                _angularAccelerationX = value;
                OnPropertyChanged("AngularAccelerationX");
            }
        }

        public float AngularAccelerationY
        {
            get
            {
                return _angularAccelerationY;
            }
            set
            {
                _angularAccelerationY = value;
                OnPropertyChanged("AngularAccelerationY");
            }
        }

        public float AngularAccelerationZ
        {
            get
            {
                return _angularAccelerationZ;
            }
            set
            {
                _angularAccelerationZ = value;
                OnPropertyChanged("AngularAccelerationZ");
            }
        }

        public float FrontWheelsAngle
        {
            get
            {
                return _frontWheelsAngle;
            }
            set
            {
                _frontWheelsAngle = value;
                OnPropertyChanged("FrontWheelsAngle");
            }
        }

        public PacketMotionData()
        {
            Header = new PacketHeader();
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
