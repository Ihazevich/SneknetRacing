using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Model
{
    public class EventDataDetails : INotifyPropertyChanged
    {
        #region Enums
        public enum PenaltyTypes
        {
            DriveThrough,
            StopGo,
            GridPenalty,
            PenaltyReminder,
            TimePenalty,
            Warning,
            Disqualified,
            RemovedFromFormationLap,
            ParkedTooLongTimer,
            TyreRegulations,
            ThisLapInvalidated,
            ThisAndNextLapInvalidated,
            ThisLapInvalidatedWithoutReason,
            ThisAndNextLapInvalidatedWithoutReason,
            ThisAndPreviousLapInvalidated,
            ThisAndPreviousLapInvalidatedWithoutReason,
            Retired,
            BlackFlagTimer
        }

        public enum InfringementTypes
        {
            BlockingBySlowDriving,
            BlockingByWrongWayDriving
            // ... a lot more
        }
        #endregion

        #region Fields
        // FastestLap
        private byte _fastestLapVehicleIdx; // Vehicle index of car achieving fastest lap
        private float _fastestLapLapTime;    // Lap time is in seconds

        // Retirement
        private byte _retirementVehicleIdx; // Vehicle index of car retiring

        // TeamMateInPits
        private byte _teammateInPitsVehicleIdx; // Vehicle index of team mate

        // RaceWinner
        private byte _raceWinnerVehicleIdx; // Vehicle index of the race winner

        // Penalty
        private byte _penaltyType;                  // Penalty type – see Appendices
        private byte _penaltyInfringementType;     // Infringement type – see Appendices
        private byte _penaltyVehicleIdx;           // Vehicle index of the car the penalty is applied to
        private byte _penaltyOtherVehicleIdx;      // Vehicle index of the other car involved
        private byte _penaltyTime;                 // Time gained, or time spent doing action in seconds
        private byte _penaltyLapNum;               // Lap the penalty occurred on
        private byte _penaltyPlacesGained;         // Number of places gained by this

        // SpeedTrap
        private byte _speedTrapVehicleIdx; // Vehicle index of the vehicle triggering speed trap
        private float _speedTrapSpeed;      // Top speed achieved in kilometres per hour
        #endregion

        #region Properties
        public byte FastestLapVehicleIdx
        {
            get
            {
                return _fastestLapVehicleIdx;
            }
            set
            {
                _fastestLapVehicleIdx = value;
                OnPropertyChanged("FastestLapVehicleIdx");
            }
        }
        public float FastestLapLapTime
        {
            get
            {
                return _fastestLapLapTime;
            }
            set
            {
                _fastestLapLapTime = value;
                OnPropertyChanged("FastestLapLapTime");
            }
        }
        public byte RetirementVehicleIdx
        {
            get
            {
                return _retirementVehicleIdx;
            }
            set
            {
                _retirementVehicleIdx = value;
                OnPropertyChanged("RetirementVehicleIdx");
            }
        }
        public byte TeammateInPitsVehicleIdx
        {
            get
            {
                return _teammateInPitsVehicleIdx;
            }
            set
            {
                _teammateInPitsVehicleIdx = value;
                OnPropertyChanged("TeammateInPitsVehicleIdx");
            }
        }
        public byte RaceWinnerVehicleIdx
        {
            get
            {
                return _raceWinnerVehicleIdx;
            }
            set
            {
                _raceWinnerVehicleIdx = value;
                OnPropertyChanged("RaceWinnerVehicleIdx");
            }
        }
        public byte PenaltyType
        {
            get
            {
                return _penaltyType;
            }
            set
            {
                _penaltyType = value;
                OnPropertyChanged("PenaltyType");
            }
        }
        public byte PenaltyInfringementType
        {
            get
            {
                return _penaltyInfringementType;
            }
            set
            {
                _penaltyInfringementType = value;
                OnPropertyChanged("PenaltyType");
            }
        }
        public byte PenaltyVehicleIdx
        {
            get
            {
                return _penaltyVehicleIdx;
            }
            set
            {
                _penaltyVehicleIdx = value;
                OnPropertyChanged("PenaltyVehicleIdx");
            }
        }
        public byte PenaltyOtherVehicleIdx
        {
            get
            {
                return _penaltyOtherVehicleIdx;
            }
            set
            {
                _penaltyOtherVehicleIdx = value;
                OnPropertyChanged("PenaltyOtherVehicleIdx");
            }
        }
        public byte PenaltyTime
        {
            get
            {
                return _penaltyTime;
            }
            set
            {
                _penaltyTime= value;
                OnPropertyChanged("PenaltyTime");
            }
        }
        public byte PenaltyLapNum
        {
            get
            {
                return _penaltyLapNum;
            }
            set
            {
                _penaltyLapNum= value;
                OnPropertyChanged("PenaltyLapNum");
            }
        }
        public byte PenaltyPlacesGained
        {
            get
            {
                return _penaltyPlacesGained;
            }
            set
            {
                _penaltyPlacesGained = value;
                OnPropertyChanged("PenaltyPlacesGained");
            }
        }
        public byte SpeedTrapVehicleIdx
        {
            get
            {
                return _speedTrapVehicleIdx;
            }
            set
            {
                _speedTrapVehicleIdx = value;
                OnPropertyChanged("SpeedTrapVehicleIdx");
            }
        }
        public float SpeedTrapSpeed
        {
            get
            {
                return _speedTrapSpeed;
            }
            set
            {
                _speedTrapSpeed = value;
                OnPropertyChanged("SpeedTrapSpeed");
            }
        }
        #endregion

        public EventDataDetails()
        {
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
}
