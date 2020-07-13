
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Model
{
    public class FinalClassificationData : INotifyPropertyChanged
    {
        #region Fields
        private byte _position;              // Finishing position
        private byte _numLaps;               // Number of laps completed
        private byte _gridPosition;          // Grid position of the car
        private byte _points;                // Number of points scored
        private byte _numPitStops;           // Number of pit stops made
        private byte _resultStatus;          // Result status - 0 = invalid, 1 = inactive, 2 = active
                                       // 3 = finished, 4 = disqualified, 5 = not classified
                                       // 6 = retired
        private float _bestLapTime;           // Best lap time of the session in seconds
        private double _totalRaceTime;         // Total race time in seconds without penalties
        private byte _penaltiesTime;         // Total penalties accumulated in seconds
        private byte _numPenalties;          // Number of penalties applied to this driver
        private byte _numTyreStints;         // Number of tyres stints up to maximum
        private byte[] _tyreStintsActual;   // Actual tyres used by this driver
        private byte[] _tyreStintsVisual;   // Visual tyres used by this driver
        #endregion

        #region Properties
        public byte Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }
        public byte NumLaps
        {
            get
            {
                return _numLaps;
            }
            set
            {
                _numLaps = value;
                OnPropertyChanged("NumLaps");
            }
        }
        public byte GridPosition
        {
            get
            {
                return _gridPosition;
            }
            set
            {
                _gridPosition = value;
                OnPropertyChanged("GridPosition");
            }
        }
        public byte Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
                OnPropertyChanged("Points");
            }
        }
        public byte NumPitStops
        {
            get
            {
                return _numPitStops;
            }
            set
            {
                _numPitStops = value;
                OnPropertyChanged("NumPitStops");
            }
        }
        public byte ResultStatus
        {
            get
            {
                return _resultStatus;
            }
            set
            {
                _resultStatus = value;
                OnPropertyChanged("ResultStatus");
            }
        }
        public float BestLapTime
        {
            get
            {
                return _bestLapTime;
            }
            set
            {
                _bestLapTime = value;
                OnPropertyChanged("BestLapTime");
            }
        }
        public double TotalRaceTime
        {
            get
            {
                return _totalRaceTime;
            }
            set
            {
                _totalRaceTime = value;
                OnPropertyChanged("TotalRaceTime");
            }
        }
        public byte PenaltiesTime
        {
            get
            {
                return _penaltiesTime;
            }
            set
            {
                _penaltiesTime = value;
                OnPropertyChanged("PenaltiesTime");
            }
        }
        public byte NumPenalties
        {
            get
            {
                return _numPenalties;
            }
            set
            {
                _numPenalties = value;
                OnPropertyChanged("NumPenalties");
            }
        }
        public byte NumTyreStints
        {
            get
            {
                return _numTyreStints;
            }
            set
            {
                _numTyreStints = value;
                OnPropertyChanged("NumTyreStints");
            }
        }
        public byte[] TyreStintsActual
        {
            get
            {
                return _tyreStintsActual;
            }
            set
            {
                _tyreStintsActual = value;
                OnPropertyChanged("TyreStintsActual");
            }
        }
        public byte[] TyreStintsVisual
        {
            get
            {
                return _tyreStintsVisual;
            }
            set
            {
                _tyreStintsVisual = value;
                OnPropertyChanged("TyreStintsVisual");
            }
        }
        #endregion

        public FinalClassificationData()
        {
            TyreStintsActual = new byte[8];
            TyreStintsVisual = new byte[8];
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
