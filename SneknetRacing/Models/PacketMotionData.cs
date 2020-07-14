using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketMotionData : BaseModel
    {
        #region Fields

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

        #endregion

        #region Properties

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

        #endregion

        public PacketMotionData()
        {
            CarMotionData = new CarMotionData[22];
            SuspensionPosition = new float[4];
        }

        public override void Desserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    Header.PacketFormat = reader.ReadUInt16();
                    Header.GameMajorVersion = reader.ReadByte();
                    Header.GameMinorVersion = reader.ReadByte();
                    Header.PacketVersion = reader.ReadByte();
                    Header.PacketID = reader.ReadByte();
                    Header.SessionUID = reader.ReadUInt64();
                    Header.SessionTime = reader.ReadSingle();
                    Header.FrameIdentifier = reader.ReadUInt32();
                    Header.PlayerCarIndex = reader.ReadByte();
                    Header.SecondaryPlayerCarIndex = reader.ReadByte();

                    for(int i = 0; i < 22; i++)
                    {
                        CarMotionData[i] = new CarMotionData
                        {
                            WorldPositionX = reader.ReadSingle(),
                            WorldPositionZ = reader.ReadSingle(),
                            WorldPositionY = reader.ReadSingle(),
                            WorldVelocityX = reader.ReadSingle(),
                            WorldVelocityY = reader.ReadSingle(),
                            WorldVelocityZ = reader.ReadSingle(),
                            WorldForwardDirX = reader.ReadInt16(),
                            WorldForwardDirY = reader.ReadInt16(),
                            WorldForwardDirZ = reader.ReadInt16(),
                            WorldRightDirX = reader.ReadInt16(),
                            WorldRightDirY = reader.ReadInt16(),
                            WorldRightDirZ = reader.ReadInt16(),
                            GForceLateral = reader.ReadSingle(),
                            GForceLongitudinal = reader.ReadSingle(),
                            GForceVertical = reader.ReadSingle(),
                            Yaw = reader.ReadSingle(),
                            Pitch = reader.ReadSingle(),
                            Roll = reader.ReadSingle()
                        };
                    }

                    SuspensionPosition = new float[4];
                    SuspensionPosition[0] = reader.ReadSingle();
                    SuspensionPosition[1] = reader.ReadSingle();
                    SuspensionPosition[2] = reader.ReadSingle();
                    SuspensionPosition[3] = reader.ReadSingle();

                    SuspensionAcceleration = new float[4];
                    SuspensionAcceleration[0] = reader.ReadSingle();
                    SuspensionAcceleration[1] = reader.ReadSingle();
                    SuspensionAcceleration[2] = reader.ReadSingle();
                    SuspensionAcceleration[3] = reader.ReadSingle();

                    SuspensionVelocity = new float[4];
                    SuspensionVelocity[0] = reader.ReadSingle();
                    SuspensionVelocity[1] = reader.ReadSingle();
                    SuspensionVelocity[2] = reader.ReadSingle();
                    SuspensionVelocity[3] = reader.ReadSingle();

                    WheelSpeed = new float[4];
                    WheelSpeed[0] = reader.ReadSingle();
                    WheelSpeed[1] = reader.ReadSingle();
                    WheelSpeed[2] = reader.ReadSingle();
                    WheelSpeed[3] = reader.ReadSingle();

                    WheelSlip = new float[4];
                    WheelSlip[0] = reader.ReadSingle();
                    WheelSlip[1] = reader.ReadSingle();
                    WheelSlip[2] = reader.ReadSingle();
                    WheelSlip[3] = reader.ReadSingle();

                    LocalVelocityX = reader.ReadSingle();
                    LocalVelocityY = reader.ReadSingle();
                    LocalVelocityZ = reader.ReadSingle();

                    AngularVelocityX = reader.ReadSingle();
                    AngularVelocityY = reader.ReadSingle();
                    AngularVelocityZ = reader.ReadSingle();

                    AngularAccelerationX = reader.ReadSingle();
                    AngularAccelerationY = reader.ReadSingle();
                    AngularAccelerationZ = reader.ReadSingle();

                    FrontWheelsAngle = reader.ReadSingle();
                }
            }
        }
    }
}
