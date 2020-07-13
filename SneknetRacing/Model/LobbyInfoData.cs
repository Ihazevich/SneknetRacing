using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Model
{
    public class LobbyInfoData : INotifyPropertyChanged
    {
        #region Fields
        private byte _aiControlled;            // Whether the vehicle is AI (1) or Human (0) controlled
        private byte _teamId;                  // Team id - see appendix (255 if no team currently selected)
        private byte _nationality;             // Nationality of the driver
        char[] _name;                // Name of participant in UTF-8 format – null terminated
                                      // Will be truncated with ... (U+2026) if too long
        private byte _readyStatus;             // 0 = not ready, 1 = ready, 2 = spectating
        #endregion

        #region Properties
        public byte AIControlled
        {
            get
            {
                return _aiControlled;
            }
            set
            {
                _aiControlled = value;
                OnPropertyChanged("AIControlled");
            }
        }
        public byte TeamID
        {
            get
            {
                return _teamId;
            }
            set
            {
                _teamId = value;
                OnPropertyChanged("TeamID");
            }
        }
        public byte Nationality
        {
            get
            {
                return _nationality;
            }
            set
            {
                _nationality = value;
                OnPropertyChanged("Nationality");
            }
        }
        public char[] Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public byte ReadyStatus
        {
            get
            {
                return _readyStatus;
            }
            set
            {
                _readyStatus = value;
                OnPropertyChanged("ReadyStatus");
            }
        }
        #endregion

        public LobbyInfoData()
        {
            Name = new char[48];
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
