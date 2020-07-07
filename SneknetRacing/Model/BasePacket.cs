using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace SneknetRacing.Model
{
    public class BasePacket : INotifyPropertyChanged
    {
        // Header
        private UInt16 m_packetFormat;       // 2019
        private byte m_gameMajorVersion;     // Game major version - "X.00"
        private byte m_gameMinorVersion;     // Game minor version - "1.XX"
        private byte m_packetVersion;        // Version of this packet type, all start from 1
        private byte m_packetId;             // Identifier for the packet type, see below
        private UInt64 m_sessionUID;         // Unique identifier for the session
        private float m_sessionTime;         // Session timestamp
        private uint m_frameIdentifier;      // Identifier for the frame the data was retrieved on
        private byte m_playerCarIndex;       // Index of player's car in the array

        // Getters
        public UInt16 M_packetFormat
        {
            get
            {
                return m_packetFormat;
            }
            set
            {
                m_packetFormat = value;
                OnPropertyChanged("PacketFormat");
            }
        }
        public byte M_gameMajorVersion
        {
            get
            {
                return m_gameMajorVersion;
            }
            set
            {
                m_gameMajorVersion = value;
                OnPropertyChanged("GameMajorVersion");
            }
        }
        public byte M_gameMinorVersion
        {
            get
            {
                return m_gameMinorVersion;
            }
            set
            {
                m_gameMinorVersion = value;
                OnPropertyChanged("GameMinorVersion");
            }
        }
        public byte M_packetVersion
        {
            get
            {
                return m_packetVersion;
            }
            set
            {
                m_packetVersion = value;
                OnPropertyChanged("PacketVersion");
            }
        }
        public byte M_packetId
        {
            get
            {
                return m_packetId;
            }
            set
            {
                m_packetId = value;
                OnPropertyChanged("PacketID");
            }
        }
        public UInt64 M_sessionUID
        {
            get
            {
                return m_sessionUID;
            }
            set
            {
                m_sessionUID = value;
                OnPropertyChanged("SessionUID");
            }
        }
        public float M_sessionTime
        {
            get
            {
                return m_sessionTime;
            }
            set
            {
                m_sessionTime = value;
                OnPropertyChanged("PacketFormat");
            }
        }
        public uint M_frameIdentifier
        {
            get
            {
                return m_frameIdentifier; 
            }
            set
            {
                m_frameIdentifier = value;
                OnPropertyChanged("FrameIdentifier");
            }
        }
        public byte M_playerCarIndex
        {
            get
            {
                return m_playerCarIndex;
            }
            set
            {
                m_playerCarIndex = value;
                OnPropertyChanged("PlayerCarIndex");
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
