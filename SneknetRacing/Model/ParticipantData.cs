using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Model
{
    public class ParticipantData : INotifyPropertyChanged
    {
        #region Fields
        private byte _aiControlled;           // Whether the vehicle is AI (1) or Human (0) controlled
        private byte _driverID;               // Driver id - see appendix
        private byte _teamID;                 // Team id - see appendix
        private byte _raceNumber;             // Race number of the car
        private byte _nationality;            // Nationality of the driver
        char[] _name;                         // Name of participant in UTF-8 format – null terminated
                                              // Will be truncated with … (U+2026) if too long
        private byte _yourTelemetry;          // The player's UDP setting, 0 = restricted, 1 = public
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
        public byte DriverID
        {
            get
            {
                return _driverID;
            }
            set
            {
                _driverID = value;
                OnPropertyChanged("DriverID");
            }
        }
        public byte TeamID
        {
            get
            {
                return _teamID;
            }
            set
            {
                _teamID = value;
                OnPropertyChanged("TeamID");
            }
        }
        public byte RaceNumber
        {
            get
            {
                return _raceNumber;
            }
            set
            {
                _raceNumber = value;
                OnPropertyChanged("RaceNumber");
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
        public byte YourTelemetry
        {
            get
            {
                return _yourTelemetry;
            }
            set
            {
                _yourTelemetry= value;
                OnPropertyChanged("YourTelemetry");
            }
        }
        #endregion

        public ParticipantData()
        {
            Name = new char[4];
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
