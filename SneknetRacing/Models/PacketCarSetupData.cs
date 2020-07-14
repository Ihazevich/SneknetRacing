using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketCarSetupData : BaseModel
    {
        #region Fields
        private PacketHeader _header;
        private CarSetupData[] _carSetups;
        #endregion

        #region Properties
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
        public CarSetupData[] CarSetups
        {
            get
            {
                return _carSetups;
            }
            set
            {
                _carSetups = value;
                OnPropertyChanged("CarSetups");
            }
        }
        #endregion
        
        public PacketCarSetupData()
        {
            CarSetups = new CarSetupData[22];
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

                    for (int i = 0; i < 22; i++)
                    {
                        CarSetups[i] = new CarSetupData()
                        {
                            FrontWing = reader.ReadByte(),
                            RearWing = reader.ReadByte(),
                            OnThrottle = reader.ReadByte(),
                            OffThrottle = reader.ReadByte(),
                            FrontCamber = reader.ReadSingle(),
                            RearCamber = reader.ReadSingle(),
                            FrontToe = reader.ReadSingle(),
                            RearToe = reader.ReadSingle(),
                            FrontSuspension = reader.ReadByte(),
                            RearSuspension = reader.ReadByte(),
                            FrontAntiRollBar = reader.ReadByte(),
                            RearAntiRollBar = reader.ReadByte(),
                            FrontSuspensionHeight = reader.ReadByte(),
                            RearSuspensionHeight = reader.ReadByte(),
                            BrakePressure = reader.ReadByte(),
                            BrakeBias = reader.ReadByte(),
                            RearLeftTyrePressure = reader.ReadSingle(),
                            RearRightTyrePressure = reader.ReadSingle(),
                            FrontLeftTyrePressure = reader.ReadSingle(),
                            FrontRightTyrePressure = reader.ReadSingle(),
                            Ballast = reader.ReadByte(),
                            FuelLoad = reader.ReadSingle()
                        };
                    }
                }
            }
        }
    }
}
