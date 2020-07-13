using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Model
{
    public class PacketCarStatusData : INotifyPropertyChanged
    {
        #region Fields
        private PacketHeader _header;
        private CarStatusData[] _carStatusData;
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
        public CarStatusData[] CarStatusData
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
        #endregion

        public PacketCarStatusData()
        {
            CarStatusData = new CarStatusData[22];
        }

        public void Desserialize(byte[] data)
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
                        CarStatusData[i] = new CarStatusData()
                        {
                            TractionControl = reader.ReadByte(),
                            AntiLockBrakes = reader.ReadByte(),
                            FuelMix = reader.ReadByte(),
                            FrontBrakeBias = reader.ReadByte(),
                            PitLimiterStatus = reader.ReadByte(),
                            FuelInTank = reader.ReadSingle(),
                            FuelCapacity = reader.ReadSingle(),
                            FuelRemainingLaps = reader.ReadSingle(),
                            MaxRPM = reader.ReadUInt16(),
                            IdleRPM = reader.ReadUInt16(),
                            MaxGears = reader.ReadByte(),
                            DrsAllowed = reader.ReadByte(),
                            DrsActivationDistance = reader.ReadUInt16(),
                            TyresWear = reader.ReadBytes(4),
                            ActualTyreCompound = reader.ReadByte(),
                            VisualTyreCompound = reader.ReadByte(),
                            TyresAgeLaps = reader.ReadByte(),
                            TyresDamage = reader.ReadBytes(4),
                            FrontLeftWingDamage = reader.ReadByte(),
                            FrontRightWingDamage = reader.ReadByte(),
                            RearWingDamage = reader.ReadByte(),
                            DrsFault = reader.ReadByte(),
                            EngineDamage = reader.ReadByte(),
                            GearBoxDamage = reader.ReadByte(),
                            VehicleFiaFlags = reader.ReadSByte(),
                            ErsStoreEnergy = reader.ReadSingle(),
                            ErsDeployMode = reader.ReadByte(),
                            ErsHarvestedThisLapMGUK = reader.ReadSingle(),
                            ErsHarvestedThisLapMGUH = reader.ReadSingle(),
                            ErsDeployedThisLap = reader.ReadSingle()
                        };
                    }
                }
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
