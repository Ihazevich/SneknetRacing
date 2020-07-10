﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Model
{
    public class PacketMotionData : INotifyPropertyChanged
    {
        private string _info;

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

        public string Info
        {
            get
            {
                return _info;
            }
            set
            {
                _info = value;
                OnPropertyChanged("Info");
            }
        }

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

        public void Desserialize(byte[] data)
        {
            PacketMotionData result = new PacketMotionData();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    Header.PacketFormat = reader.ReadUInt16();
                    Header.GameMajorVersion = reader.ReadByte();
                    Header.GameMinorVersion = reader.ReadByte();
                    Header.PacketVersion = reader.ReadByte();
                    Header.PacketID = reader.ReadByte();
                    Header.SessionUID = reader.ReadUInt32();
                    Header.SessionTime = (float)reader.ReadDouble();
                    Header.FrameIdentifier = reader.ReadUInt32();
                    Header.PlayerCarIndex = reader.ReadByte();
                    Header.SecondaryPlayerCarIndex = reader.ReadByte();
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
