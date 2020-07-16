using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketCarStatusData : BaseModel
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
            Header = new PacketHeader();
            CarStatusData = new CarStatusData[22];
        }

        public override BaseModel Desserialize(byte[] data)
        {
            PacketCarStatusData temp = new PacketCarStatusData();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    temp.Header.PacketFormat = reader.ReadUInt16();
                    temp.Header.GameMajorVersion = reader.ReadByte();
                    temp.Header.GameMinorVersion = reader.ReadByte();
                    temp.Header.PacketVersion = reader.ReadByte();
                    temp.Header.PacketID = reader.ReadByte();
                    temp.Header.SessionUID = reader.ReadUInt64();
                    temp.Header.SessionTime = reader.ReadSingle();
                    temp.Header.FrameIdentifier = reader.ReadUInt32();
                    temp.Header.PlayerCarIndex = reader.ReadByte();
                    temp.Header.SecondaryPlayerCarIndex = reader.ReadByte();

                    for(int i = 0; i < 22; i++)
                    {
                        temp.CarStatusData[i] = new CarStatusData()
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
            return temp;
        }
    }
}
