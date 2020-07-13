using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketLapData : BaseModel
    {
        #region Fields
        private PacketHeader _header;             // Header
        private LapData[] _lapData;        // Lap data for all cars on track
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
        public LapData[] LapData
        {
            get
            {
                return _lapData;
            }
            set
            {
                _lapData = value;
                OnPropertyChanged("LapData");
            }
        }
        #endregion

        public PacketLapData()
        {
            LapData = new LapData[22];
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
                        LapData[i] = new LapData()
                        {
                            LastLapTime = reader.ReadSingle(),
                            CurrentLapTime = reader.ReadSingle(),
                            Sector1TimeInMS = reader.ReadUInt16(),
                            Sector2TimeInMS = reader.ReadUInt16(),
                            BestLapTime = reader.ReadSingle(),
                            BestLapNum = reader.ReadByte(),
                            BestLapSector1TimeInMS = reader.ReadUInt16(),
                            BestLapSector2TimeInMS = reader.ReadUInt16(),
                            BestLapSector3TimeInMS = reader.ReadUInt16(),
                            BestOverallSector1TimeInMS = reader.ReadUInt16(),
                            BestOverallSector1LapNum = reader.ReadByte(),
                            BestOverallSector2TimeInMS = reader.ReadUInt16(),
                            BestOverallSector2LapNum = reader.ReadByte(),
                            BestOverallSector3TimeInMS = reader.ReadUInt16(),
                            BestOverallSector3LapNum = reader.ReadByte(),
                            LapDistance = reader.ReadSingle(),
                            TotalDistance = reader.ReadSingle(),
                            SafetyCarDelta = reader.ReadSingle(),
                            CarPosition = reader.ReadByte(),
                            CurrentLapNum = reader.ReadByte(),
                            PitStatus = reader.ReadByte(),
                            Sector = reader.ReadByte(),
                            CurrentLapInvalid = reader.ReadByte(),
                            Penalties = reader.ReadByte(),
                            GridPosition = reader.ReadByte(),
                            DriverStatus = reader.ReadByte(),
                            ResultStatus = reader.ReadByte()
                        };
                    }
                }
            }
        }
    }
}
