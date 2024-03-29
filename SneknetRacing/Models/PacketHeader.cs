﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;


namespace SneknetRacing.Models
{
    public class PacketHeader : BaseModel
    {
        #region Fields
        private UInt16 _packetFormat = 0;       // 2019
        private byte _gameMajorVersion = 0;     // Game major version - "X.00"
        private byte _gameMinorVersion = 0;     // Game minor version - "1.XX"
        private byte _packetVersion = 0;        // Version of this packet type, all start from 1
        private byte _packetID = 0;             // Identifier for the packet type, see below
        private UInt64 _sessionUID = 0;         // Unique identifier for the session    
        private float _sessionTime = 0;         // Session timestamp
        private uint _frameIdentifier = 0;      // Identifier for the frame the data was retrieved on
        private byte _playerCarIndex = 0;       // Index of player's car in the array
        private byte _secondaryPlayerCarIndex = 0;  // Index of secondary player's car in the array (splitscreen)
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
        public override BaseModel Desserialize(byte[] data)
        {
            PacketHeader temp = new PacketHeader();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    temp.PacketFormat = reader.ReadUInt16();
                    temp.GameMajorVersion = reader.ReadByte();
                    temp.GameMinorVersion = reader.ReadByte();
                    temp.PacketVersion = reader.ReadByte();
                    temp.PacketID = reader.ReadByte();
                    temp.SessionUID = reader.ReadUInt64();
                    temp.SessionTime = reader.ReadSingle();
                    temp.FrameIdentifier = reader.ReadUInt32();
                    temp.PlayerCarIndex = reader.ReadByte();
                    temp.SecondaryPlayerCarIndex = reader.ReadByte();
                }
            }
            return temp;
        }
    }
}
