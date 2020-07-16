using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Models
{
    public class CarStatusData : BaseModel
    {
        #region Fields
        private byte _tractionControl;          // 0 (off) - 2 (high)
        private byte _antiLockBrakes;           // 0 (off) - 1 (on)
        private byte _fuelMix;                  // Fuel mix - 0 = lean, 1 = standard, 2 = rich, 3 = max
        private byte _frontBrakeBias;           // Front brake bias (percentage)
        private byte _pitLimiterStatus;         // Pit limiter status - 0 = off, 1 = on
        private float _fuelInTank;               // Current fuel mass
        private float _fuelCapacity;             // Fuel capacity
        private float _fuelRemainingLaps;        // Fuel remaining in terms of laps (value on MFD)
        private UInt16 _maxRPM;                   // Cars max RPM, point of rev limiter
        private UInt16 _idleRPM;                  // Cars idle RPM
        private byte _maxGears;                 // Maximum number of gears
        private byte _drsAllowed;               // 0 = not allowed, 1 = allowed, -1 = unknown


        // Added in Beta3:
        private UInt16 _drsActivationDistance;    // 0 = DRS not available, non-zero - DRS will be available
                                                  // in [X] metres

        private byte[] _tyresWear;             // Tyre wear percentage
        private byte _actualTyreCompound;     // F1 Modern - 16 = C5, 17 = C4, 18 = C3, 19 = C2, 20 = C1
                                              // 7 = inter, 8 = wet
                                              // F1 Classic - 9 = dry, 10 = wet
                                              // F2 – 11 = super soft, 12 = soft, 13 = medium, 14 = hard
                                              // 15 = wet
        private byte _visualTyreCompound;        // F1 visual (can be different from actual compound)
                                                 // 16 = soft, 17 = medium, 18 = hard, 7 = inter, 8 = wet
                                                 // F1 Classic – same as above
                                                 // F2 – same as above
        private byte _tyresAgeLaps;             // Age in laps of the current set of tyres
        private byte[] _tyresDamage;           // Tyre damage (percentage)
        private byte _frontLeftWingDamage;      // Front left wing damage (percentage)
        private byte _frontRightWingDamage;     // Front right wing damage (percentage)
        private byte _rearWingDamage;           // Rear wing damage (percentage)

        // Added Beta 3:
        private byte _drsFault;                 // Indicator for DRS fault, 0 = OK, 1 = fault

        private byte _engineDamage;             // Engine damage (percentage)
        private byte _gearBoxDamage;            // Gear box damage (percentage)
        private sbyte _vehicleFiaFlags;          // -1 = invalid/unknown, 0 = none, 1 = green
                                         // 2 = blue, 3 = yellow, 4 = red
        private float _ersStoreEnergy;           // ERS energy store in Joules
        private byte _ersDeployMode;            // ERS deployment mode, 0 = none, 1 = medium
                                          // 2 = overtake, 3 = hotlap
        private float _ersHarvestedThisLapMGUK;  // ERS energy harvested this lap by MGU-K
        private float _ersHarvestedThisLapMGUH;  // ERS energy harvested this lap by MGU-H
        private float _ersDeployedThisLap;       // ERS energy deployed this lap
        #endregion

        #region Properties
        public byte TractionControl
        {
            get
            {
                return _tractionControl;
            }
            set
            {
                _tractionControl = value;
                OnPropertyChanged("TractionControl");
            }
        }
        public byte AntiLockBrakes
        {
            get
            {
                return _antiLockBrakes;
            }
            set
            {
                _antiLockBrakes = value;
                OnPropertyChanged("AntiLockBrakes");
            }
        }
        public byte FuelMix
        {
            get
            {
                return _fuelMix;
            }
            set
            {
                _fuelMix = value;
                OnPropertyChanged("FuelMix");
            }
        }
        public byte FrontBrakeBias
        {
            get
            {
                return _frontBrakeBias;
            }
            set
            {
                _frontBrakeBias = value;
                OnPropertyChanged("FrontBrakeBias");
            }
        }
        public byte PitLimiterStatus
        {
            get
            {
                return _pitLimiterStatus;
            }
            set
            {
                _pitLimiterStatus = value;
                OnPropertyChanged("PitLimiterStatus");
            }
        }
        public float FuelInTank
        {
            get
            {
                return _fuelInTank;
            }
            set
            {
                _fuelInTank = value;
                OnPropertyChanged("FuelInTank");
            }
        }
        public float FuelCapacity
        {
            get
            {
                return _fuelCapacity;
            }
            set
            {
                _fuelCapacity = value;
                OnPropertyChanged("FuelCapacity");
            }
        }
        public float FuelRemainingLaps
        {
            get
            {
                return _fuelRemainingLaps;
            }
            set
            {
                _fuelRemainingLaps = value;
                OnPropertyChanged("FuelRemainingLaps");
            }
        }
        public UInt16 MaxRPM
        {
            get
            {
                return _maxRPM;
            }
            set
            {
                _maxRPM = value;
                OnPropertyChanged("MaxRPM");
            }
        }
        public UInt16 IdleRPM
        {
            get
            {
                return _idleRPM;
            }
            set
            {
                _idleRPM = value;
                OnPropertyChanged("IdleRPM");
            }
        }
        public byte MaxGears
        {
            get
            {
                return _maxGears;
            }
            set
            {
                _maxGears = value;
                OnPropertyChanged("MaxGears");
            }
        }
        public byte DrsAllowed
        {
            get
            {
                return _drsAllowed;
            }
            set
            {
                _drsAllowed = value;
                OnPropertyChanged("DrsAllowed");
            }
        }
        public UInt16 DrsActivationDistance
        {
            get
            {
                return _drsActivationDistance;
            }
            set
            {
                _drsActivationDistance = value;
                OnPropertyChanged("DrsActivationDistance");
            }
        }
        public byte[] TyresWear
        {
            get
            {
                return _tyresWear;
            }
            set
            {
                _tyresWear = value;
                OnPropertyChanged("TyresWear");
            }
        }
        public byte ActualTyreCompound
        {
            get
            {
                return _actualTyreCompound;
            }
            set
            {
                _actualTyreCompound = value;
                OnPropertyChanged("ActualTyreCompound");
            }
        }
        public byte VisualTyreCompound
        {
            get
            {
                return _visualTyreCompound;
            }
            set
            {
                _visualTyreCompound = value;
                OnPropertyChanged("VisualTyreCompound");
            }
        }
        public byte TyresAgeLaps
        {
            get
            {
                return _tyresAgeLaps;
            }
            set
            {
                _tyresAgeLaps = value;
                OnPropertyChanged("TyresAgeLaps");
            }
        }
        public byte[] TyresDamage
        {
            get
            {
                return _tyresDamage;
            }
            set
            {
                _tyresDamage = value;
                OnPropertyChanged("TyresDamage");
            }
        }
        public byte FrontLeftWingDamage
        {
            get
            {
                return _frontLeftWingDamage;
            }
            set
            {
                _frontLeftWingDamage = value;
                OnPropertyChanged("FrontLeftWingDamage");
            }
        }
        public byte FrontRightWingDamage
        {
            get
            {
                return _frontRightWingDamage;
            }
            set
            {
                _frontRightWingDamage = value;
                OnPropertyChanged("FrontRightWingDamage");
            }
        }
        public byte RearWingDamage
        {
            get
            {
                return _rearWingDamage;
            }
            set
            {
                _rearWingDamage = value;
                OnPropertyChanged("RearWingDamage");
            }
        }
        public byte DrsFault
        {
            get
            {
                return _drsFault;
            }
            set
            {
                _drsFault = value;
                OnPropertyChanged("DrsFault");
            }
        }
        public byte EngineDamage
        {
            get
            {
                return _engineDamage;
            }
            set
            {
                _engineDamage = value;
                OnPropertyChanged("EngineDamage");
            }
        }
        public byte GearBoxDamage
        {
            get
            {
                return _gearBoxDamage;
            }
            set
            {
                _gearBoxDamage = value;
                OnPropertyChanged("GearBoxDamage");
            }
        }
        public sbyte VehicleFiaFlags
        {
            get
            {
                return _vehicleFiaFlags;
            }
            set
            {
                _vehicleFiaFlags = value;
                OnPropertyChanged("VehicleFiaFlags");
            }
        }
        public float ErsStoreEnergy
        {
            get
            {
                return _ersStoreEnergy;
            }
            set
            {
                _ersStoreEnergy = value;
                OnPropertyChanged("ErsStoreEnergy");
            }
        }
        public byte ErsDeployMode
        {
            get
            {
                return _ersDeployMode;
            }
            set
            {
                _ersDeployMode = value;
                OnPropertyChanged("ErsDeployMode");
            }
        }
        public float ErsHarvestedThisLapMGUK
        {
            get
            {
                return _ersHarvestedThisLapMGUK;
            }
            set
            {
                _ersHarvestedThisLapMGUK = value;
                OnPropertyChanged("ErsHarvestedThisLapMGUK");
            }
        }
        public float ErsHarvestedThisLapMGUH
        {
            get
            {
                return _ersHarvestedThisLapMGUH;
            }
            set
            {
                _ersHarvestedThisLapMGUH = value;
                OnPropertyChanged("ErsHarvestedThisLapMGUH");
            }
        }
        public float ErsDeployedThisLap
        {
            get
            {
                return _ersDeployedThisLap;
            }
            set
            {
                _ersDeployedThisLap = value;
                OnPropertyChanged("ErsDeployedThisLap");
            }
        }
        #endregion

        public CarStatusData()
        {
            TyresWear = new byte[4];
            TyresDamage = new byte[4];
        }

        public override BaseModel Desserialize(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
