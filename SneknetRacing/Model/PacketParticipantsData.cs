using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Model
{
    public class PacketParticipantsData : INotifyPropertyChanged
    {
        #region Fields
        private PacketHeader _header;           // Header
        private byte _numActiveCars;  // Number of active cars in the data – should match number of
                                // cars on HUD
        private ParticipantData[] _participants;
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
        public byte NumActiveCars
        {
            get
            {
                return _numActiveCars;
            }
            set
            {
                _numActiveCars = value;
                OnPropertyChanged("NumActiveCars");
            }
        }
        public ParticipantData[] Participants
        {
            get
            {
                return _participants;
            }
            set
            {
                _participants = value;
                OnPropertyChanged("Participants");
            }
        }
        #endregion

        public PacketParticipantsData()
        {
            Participants = new ParticipantData[22];
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

                    NumActiveCars = reader.ReadByte();

                    for(int i = 0; i < 22; i++)
                    {
                        Participants[i] = new ParticipantData()
                        {
                            AIControlled = reader.ReadByte(),
                            DriverID = reader.ReadByte(),
                            TeamID = reader.ReadByte(),
                            RaceNumber = reader.ReadByte(),
                            Nationality = reader.ReadByte(),
                            Name = reader.ReadChars(4),
                            YourTelemetry = reader.ReadByte()
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
