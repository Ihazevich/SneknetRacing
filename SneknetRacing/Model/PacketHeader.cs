using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace SneknetRacing.Model
{
    public struct PacketHeaderStruct
    {
        UInt16 packetFormat;                // 2020
        byte gameMajorVersion;              // Game major version - "X.00"
        byte gameMinorVersion;              // Game minor version - "1.XX"
        byte packetVersion;                 // Version of this packet type, all start from 1
        byte packetID;                      // Identifier for the packet type, see below
        UInt64 sessionUID;                  // Unique identifier for the session
        float sessionTime;                  // Session timestamp
        UInt32 frameIdentifier;             // Identifier for the frame the data was retrieved on
        byte playerCarIndex;                // Index of player's car in the array

        // ADDED IN BETA 2: 
        byte secondaryPlayerCarIndex;       // Index of secondary player's car in the array (splitscreen)
                                            // 255 if no second player
    }

    public class PacketHeader : INotifyPropertyChanged
    {
        // Header
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

        // Getters
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
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
