using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketFinalClassificationData : BaseModel
    {
        #region Fields
        private PacketHeader _header;
        private byte _numCars;
        private FinalClassificationData[] _classificationData;
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
        public byte NumCars
        {
            get
            {
                return _numCars;
            }
            set
            {
                _numCars = value;
                OnPropertyChanged("NumCars");
            }
        }
        public FinalClassificationData[] ClassificationData
        {
            get
            {
                return _classificationData;
            }
            set
            {
                _classificationData = value;
                OnPropertyChanged("ClassificationData");
            }
        }
        #endregion

        public PacketFinalClassificationData()
        {
            Header = new PacketHeader();
            ClassificationData = new FinalClassificationData[22];
        }

        public override BaseModel Desserialize(byte[] data)
        {
            PacketFinalClassificationData temp = new PacketFinalClassificationData();
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

                    temp.NumCars = reader.ReadByte();
                    
                    for(int i = 0; i < 22; i++)
                    {
                        temp.ClassificationData[i] = new FinalClassificationData()
                        {
                            Position = reader.ReadByte(),
                            NumLaps = reader.ReadByte(),
                            GridPosition = reader.ReadByte(),
                            Points = reader.ReadByte(),
                            NumPitStops = reader.ReadByte(),
                            ResultStatus = reader.ReadByte(),
                            BestLapTime = reader.ReadSingle(),
                            TotalRaceTime = reader.ReadDouble(),
                            PenaltiesTime = reader.ReadByte(),
                            NumPenalties = reader.ReadByte(),
                            NumTyreStints = reader.ReadByte(),
                            TyreStintsActual = reader.ReadBytes(8),
                            TyreStintsVisual = reader.ReadBytes(8)
                        };
                    }
                }
            }
            return temp;
        }

    }
}
