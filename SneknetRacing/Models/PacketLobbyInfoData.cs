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
            Header = new PacketHeader();
            LobbyPlayers = new LobbyInfoData[22];
        }

        public override BaseModel Desserialize(byte[] data)
        {
            PacketLobbyInfoData temp = new PacketLobbyInfoData();
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

                    temp.NumPlayers = reader.ReadByte();

                    for (int i = 0; i < 22; i++)
                    {
                        temp.LobbyPlayers[i] = new LobbyInfoData()
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
            return temp;
        }
    }
}
