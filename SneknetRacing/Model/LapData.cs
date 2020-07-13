using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Model
{
    public class LapData : BaseModel
    {
        #region Fields
        private float _lastLapTime;               // Last lap time in seconds
        private float _currentLapTime;            // Current time around the lap in seconds

        //UPDATED in Beta 3:
        private UInt16 _sector1TimeInMS;           // Sector 1 time in milliseconds
        private UInt16 _sector2TimeInMS;           // Sector 2 time in milliseconds
        private float _bestLapTime;               // Best lap time of the session in seconds
        private byte _bestLapNum;                // Lap number best time achieved on
        private UInt16 _bestLapSector1TimeInMS;    // Sector 1 time of best lap in the session in milliseconds
        private UInt16 _bestLapSector2TimeInMS;    // Sector 2 time of best lap in the session in milliseconds
        private UInt16 _bestLapSector3TimeInMS;    // Sector 3 time of best lap in the session in milliseconds
        private UInt16 _bestOverallSector1TimeInMS;// Best overall sector 1 time of the session in milliseconds
        private byte _bestOverallSector1LapNum;  // Lap number best overall sector 1 time achieved on
        private UInt16 _bestOverallSector2TimeInMS;// Best overall sector 2 time of the session in milliseconds
        private byte _bestOverallSector2LapNum;  // Lap number best overall sector 2 time achieved on
        private UInt16 _bestOverallSector3TimeInMS;// Best overall sector 3 time of the session in milliseconds
        private byte _bestOverallSector3LapNum;  // Lap number best overall sector 3 time achieved on

        private float _lapDistance;               // Distance vehicle is around current lap in metres – could
                                                  // be negative if line hasn’t been crossed yet
        private float _totalDistance;             // Total distance travelled in session in metres – could
                                           // be negative if line hasn’t been crossed yet
        private float _safetyCarDelta;            // Delta in seconds for safety car
        private byte _carPosition;               // Car race position
        private byte _currentLapNum;             // Current lap number
        private byte _pitStatus;                 // 0 = none, 1 = pitting, 2 = in pit area
        private byte _sector;                    // 0 = sector1, 1 = sector2, 2 = sector3
        private byte _currentLapInvalid;         // Current lap invalid - 0 = valid, 1 = invalid
        private byte _penalties;                 // Accumulated time penalties in seconds to be added
        private byte _gridPosition;              // Grid position the vehicle started the race in
        private byte _driverStatus;              // Status of driver - 0 = in garage, 1 = flying lap
                                                 // 2 = in lap, 3 = out lap, 4 = on track
        private byte _resultStatus;              // Result status - 0 = invalid, 1 = inactive, 2 = active
                                                 // 3 = finished, 4 = disqualified, 5 = not classified
                                                 // 6 = retired
        #endregion

        #region Properties
        public float LastLapTime
        {
            get
            {
                return _lastLapTime;
            }
            set
            {
                _lastLapTime = value;
                OnPropertyChanged("LastLapTime");
            }
        }
        public float CurrentLapTime
        {
            get
            {
                return _currentLapTime;
            }
            set
            {
                _currentLapTime = value;
                OnPropertyChanged("CurrentLapTime");
            }
        }
        public UInt16 Sector1TimeInMS
        {
            get
            {
                return _sector1TimeInMS;
            }
            set
            {
                _sector1TimeInMS = value;
                OnPropertyChanged("Sector1TimeInMS");
            }
        }
        public UInt16 Sector2TimeInMS
        {
            get
            {
                return _sector2TimeInMS;
            }
            set
            {
                _sector2TimeInMS = value;
                OnPropertyChanged("Sector2TimeInMS");
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
        public byte BestLapNum
        {
            get
            {
                return _bestLapNum;
            }
            set
            {
                _bestLapNum = value;
                OnPropertyChanged("BestLapNum");
            }
        }
        public UInt16 BestLapSector1TimeInMS
        {
            get
            {
                return _bestLapSector1TimeInMS;
            }
            set
            {
                _bestLapSector1TimeInMS = value;
                OnPropertyChanged("BestLapSector1TimeInMS");
            }
        }
        public UInt16 BestLapSector2TimeInMS
        {
            get
            {
                return _bestLapSector2TimeInMS;
            }
            set
            {
                _bestLapSector2TimeInMS = value;
                OnPropertyChanged("BestLapSector2TimeInMS");
            }
        }
        public UInt16 BestLapSector3TimeInMS
        {
            get
            {
                return _bestLapSector3TimeInMS;
            }
            set
            {
                _bestLapSector3TimeInMS = value;
                OnPropertyChanged("BestLapSector3TimeInMS");
            }
        }
        public UInt16 BestOverallSector1TimeInMS
        {
            get
            {
                return _bestOverallSector1TimeInMS;
            }
            set
            {
                _bestOverallSector1TimeInMS = value;
                OnPropertyChanged("BestOverallSector1TimeInMS");
            }
        }
        public byte BestOverallSector1LapNum
        {
            get
            {
                return _bestOverallSector1LapNum;
            }
            set
            {
                _bestOverallSector1LapNum = value;
                OnPropertyChanged("BestOverallSector1LapNum");
            }
        }
        public UInt16 BestOverallSector2TimeInMS
        {
            get
            {
                return _bestOverallSector2TimeInMS;
            }
            set
            {
                _bestOverallSector2TimeInMS = value;
                OnPropertyChanged("BestOverallSector2TimeInMS");
            }
        }
        public byte BestOverallSector2LapNum
        {
            get
            {
                return _bestOverallSector2LapNum;
            }
            set
            {
                _bestOverallSector2LapNum = value;
                OnPropertyChanged("BestOverallSector2LapNum");
            }
        }
        public UInt16 BestOverallSector3TimeInMS
        {
            get
            {
                return _bestOverallSector3TimeInMS;
            }
            set
            {
                _bestOverallSector3TimeInMS = value;
                OnPropertyChanged("BestOverallSector3TimeInMS");
            }
        }
        public byte BestOverallSector3LapNum
        {
            get
            {
                return _bestOverallSector3LapNum;
            }
            set
            {
                _bestOverallSector2LapNum = value;
                OnPropertyChanged("BestOverallSector3LapNum");
            }
        }
        public float LapDistance
        {
            get
            {
                return _lapDistance;
            }
            set
            {
                _lapDistance = value;
                OnPropertyChanged("LapDistance");
            }
        }
        public float TotalDistance
        {
            get
            {
                return _totalDistance;
            }
            set
            {
                _totalDistance = value;
                OnPropertyChanged("TotalDistance");
            }
        }
        public float SafetyCarDelta
        {
            get
            {
                return _safetyCarDelta;
            }
            set
            {
                _safetyCarDelta = value;
                OnPropertyChanged("SafetyCarDelta");
            }
        }
        public byte CarPosition
        {
            get
            {
                return _carPosition;
            }
            set
            {
                _carPosition = value;
                OnPropertyChanged("CarPosition");
            }
        }
        public byte CurrentLapNum
        {
            get
            {
                return _currentLapNum;
            }
            set
            {
                _currentLapNum = value;
                OnPropertyChanged("CurrentLapNum");
            }
        }
        public byte PitStatus
        {
            get
            {
                return _pitStatus;
            }
            set
            {
                _pitStatus = value;
                OnPropertyChanged("PitStatus");
            }
        }
        public byte Sector
        {
            get
            {
                return _sector;
            }
            set
            {
                _sector = value;
                OnPropertyChanged("Sector");
            }
        }
        public byte CurrentLapInvalid
        {
            get
            {
                return _currentLapInvalid;
            }
            set
            {
                _currentLapInvalid = value;
                OnPropertyChanged("CurrentLapInvalid");
            }
        }
        public byte Penalties
        {
            get
            {
                return _penalties;
            }
            set
            {
                _penalties = value;
                OnPropertyChanged("Penalties");
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
        public byte DriverStatus
        {
            get
            {
                return _driverStatus;
            }
            set
            {
                _driverStatus = value;
                OnPropertyChanged("DriverStatus");
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
        #endregion
    }
}
