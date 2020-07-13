using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;


namespace SneknetRacing.Model
{
    public class PacketHeader : BaseModel
    {
        #region Fields
        private UInt16 _packetFormat;       // 2019
        private byte _gameMajorVersion;     // Game major version - "X.00"
        private byte _gameMinorVersion;     // Game minor version - "1.XX"
        private byte _packetVersion;        // Version of this packet type, all start from 1
        private byte _packetID;             // Identifier for the packet type, see below
        private UInt64 _sessionUID;         // Unique identifier for the session    
        private float _sessionTime;         // Session timestamp
        private uint _frameIdentifier;      // Identifier for the frame the data was retrieved on
        private byte _playerCarIndex;       // Index of player's car in the array
        private byte _secondaryPlayerCarIndex;  // Index of secondary player's car in the array (splitscreen)
                                                // 255 if no second player
        #endregion

        #region Properties
        public UInt16 PacketFormat
        {
            get
            {
                return _packetFormat;
            }
            set
            {
                _packetFormat = value;
                OnPropertyChanged("PacketFormat");
            }
        }
        public byte GameMajorVersion
        {
            get
            {
                return _gameMajorVersion;
            }
            set
            {
                _gameMajorVersion = value;
                OnPropertyChanged("GameMajorVersion");
            }
        }
        public byte GameMinorVersion
        {
            get
            {
                return _gameMinorVersion;
            }
            set
            {
                _gameMinorVersion = value;
                OnPropertyChanged("GameMinorVersion");
            }
        }
        public byte PacketVersion
        {
            get
            {
                return _packetVersion;
            }
            set
            {
                _packetVersion = value;
                OnPropertyChanged("PacketVersion");
            }
        }
        public byte PacketID
        {
            get
            {
                return _packetID;
            }
            set
            {
                _packetID = value;
                OnPropertyChanged("PacketID");
            }
        }
        public UInt64 SessionUID
        {
            get
            {
                return _sessionUID;
            }
            set
            {
                _sessionUID = value;
                OnPropertyChanged("SessionUID");
            }
        }
        public float SessionTime
        {
            get
            {
                return _sessionTime;
            }
            set
            {
                _sessionTime = value;
                OnPropertyChanged("PacketFormat");
            }
        }
        public uint FrameIdentifier
        {
            get
            {
                return _frameIdentifier; 
            }
            set
            {
                _frameIdentifier = value;
                OnPropertyChanged("FrameIdentifier");
            }
        }
        public byte PlayerCarIndex
        {
            get
            {
                return _playerCarIndex;
            }
            set
            {
                _playerCarIndex = value;
                OnPropertyChanged("PlayerCarIndex");
            }
        }
        public byte SecondaryPlayerCarIndex
        {
            get
            {
                return _secondaryPlayerCarIndex;
            }
            set
            {
                _secondaryPlayerCarIndex = value;
                OnPropertyChanged("SecondaryPlayerCarIndex");
            }
        }
        #endregion
        public void Desserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    PacketFormat = reader.ReadUInt16();
                    GameMajorVersion = reader.ReadByte();
                    GameMinorVersion = reader.ReadByte();
                    PacketVersion = reader.ReadByte();
                    PacketID = reader.ReadByte();
                    SessionUID = reader.ReadUInt64();
                    SessionTime = reader.ReadSingle();
                    FrameIdentifier = reader.ReadUInt32();
                    PlayerCarIndex = reader.ReadByte();
                    SecondaryPlayerCarIndex = reader.ReadByte();
                }
            }
        }
    }
}
