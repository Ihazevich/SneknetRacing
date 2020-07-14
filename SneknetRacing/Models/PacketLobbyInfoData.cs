using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketLobbyInfoData : BaseModel
    {
        #region Fields
        private PacketHeader _header;
        private byte _numPlayers;
        private LobbyInfoData[] _lobbyPlayers;
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
        public byte NumPlayers
        {
            get
            {
                return _numPlayers;
            }
            set
            {
                _numPlayers = value;
                OnPropertyChanged("NumPlayers");
            }
        }
        public LobbyInfoData[] LobbyPlayers
        {
            get
            {
                return _lobbyPlayers;
            }
            set
            {
                _lobbyPlayers = value;
                OnPropertyChanged("LobbyPlayers");
            }
        }
        #endregion

        public PacketLobbyInfoData()
        {
            LobbyPlayers = new LobbyInfoData[22];
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

                    NumPlayers = reader.ReadByte();

                    for(int i = 0; i < 22; i++)
                    {
                        LobbyPlayers[i] = new LobbyInfoData()
                        {
                            AIControlled = reader.ReadByte(),
                            TeamID = reader.ReadByte(),
                            Nationality = reader.ReadByte(),
                            Name = reader.ReadChars(48),
                            ReadyStatus = reader.ReadByte()
                        };
                    }
                }
            }
        }
    }
}
