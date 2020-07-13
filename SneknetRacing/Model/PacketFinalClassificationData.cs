using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Model
{
    public class PacketFinalClassificationData : INotifyPropertyChanged
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

        public PacketFinalClassificationData()
        {
            ClassificationData = new FinalClassificationData[22];
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

                    NumCars = reader.ReadByte();
                    
                    for(int i = 0; i < 22; i++)
                    {
                        ClassificationData[i] = new FinalClassificationData()
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
        }

        #endregion
                    #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
