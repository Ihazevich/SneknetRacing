using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketParticipantsData : BaseModel
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
            Header = new PacketHeader();
            Participants = new ParticipantData[22];
        }

        public override BaseModel Desserialize(byte[] data)
        {
            PacketParticipantsData temp = new PacketParticipantsData();
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

                    temp.NumActiveCars = reader.ReadByte();

                    for(int i = 0; i < 22; i++)
                    {
                        temp.Participants[i] = new ParticipantData()
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
            return temp;
        }
    }
}
