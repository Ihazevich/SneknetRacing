using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Models
{
    public class CarSetupData : BaseModel
    {
        #region Fields
        private byte _frontWing;                // Front wing aero
        private byte _rearWing;                 // Rear wing aero
        private byte _onThrottle;               // Differential adjustment on throttle (percentage)
        private byte _offThrottle;              // Differential adjustment off throttle (percentage)
        private float _frontCamber;              // Front camber angle (suspension geometry)
        private float _rearCamber;               // Rear camber angle (suspension geometry)
        private float _frontToe;                 // Front toe angle (suspension geometry)
        private float _rearToe;                  // Rear toe angle (suspension geometry)
        private byte _frontSuspension;          // Front suspension
        private byte _rearSuspension;           // Rear suspension
        private byte _frontAntiRollBar;         // Front anti-roll bar
        private byte _rearAntiRollBar;          // Front anti-roll bar
        private byte _frontSuspensionHeight;    // Front ride height
        private byte _rearSuspensionHeight;     // Rear ride height
        private byte _brakePressure;            // Brake pressure (percentage)
        private byte _brakeBias;                // Brake bias (percentage)
        private float _rearLeftTyrePressure;     // Rear left tyre pressure (PSI)
        private float _rearRightTyrePressure;    // Rear right tyre pressure (PSI)
        private float _frontLeftTyrePressure;    // Front left tyre pressure (PSI)
        private float _frontRightTyrePressure;   // Front right tyre pressure (PSI)
        private byte _ballast;                  // Ballast
        private float _fuelLoad;                 // Fuel load
        #endregion

        #region Properties
        public byte FrontWing
        {
            get
            {
                return _frontWing;
            }
            set
            {
                _frontWing = value;
                OnPropertyChanged("FrontWing");
            }
        }
        public byte RearWing
        {
            get
            {
                return _rearWing;
            }
            set
            {
                _rearWing = value;
                OnPropertyChanged("RearWing");
            }
        }
        public byte OnThrottle
        {
            get
            {
                return _onThrottle;
            }
            set
            {
                _onThrottle = value;
                OnPropertyChanged("OnThrottle");
            }
        }
        public byte OffThrottle
        {
            get
            {
                return _offThrottle;
            }
            set
            {
                _offThrottle = value;
                OnPropertyChanged("OffThrottle");
            }
        }
        public float FrontCamber
        {
            get
            {
                return _frontCamber;
            }
            set
            {
                _frontCamber = value;
                OnPropertyChanged("FrontCamber");
            }
        }
        public float RearCamber
        {
            get
            {
                return _rearCamber;
            }
            set
            {
                _rearCamber = value;
                OnPropertyChanged("RearCamber");
            }
        }
        public float FrontToe
        {
            get
            {
                return _frontToe;
            }
            set
            {
                _frontToe = value;
                OnPropertyChanged("FrontToe");
            }
        }
        public float RearToe
        {
            get
            {
                return _rearToe;
            }
            set
            {
                _rearToe = value;
                OnPropertyChanged("RearToe");
            }
        }
        public byte FrontSuspension
        {
            get
            {
                return _frontSuspension;
            }
            set
            {
                _frontSuspension = value;
                OnPropertyChanged("FrontSuspension");
            }
        }
        public byte RearSuspension
        {
            get
            {
                return _rearSuspension;
            }
            set
            {
                _rearSuspension = value;
                OnPropertyChanged("RearSuspension");
            }
        }
        public byte FrontAntiRollBar
        {
            get
            {
                return _frontAntiRollBar;
            }
            set
            {
                _frontAntiRollBar = value;
                OnPropertyChanged("FrontAntiRollBar");
            }
        }
        public byte RearAntiRollBar
        {
            get
            {
                return _rearAntiRollBar;
            }
            set
            {
                _rearAntiRollBar = value;
                OnPropertyChanged("RearAntiRollBar");
            }
        }
        public byte FrontSuspensionHeight
        {
            get
            {
                return _frontSuspensionHeight;
            }
            set
            {
                _frontSuspensionHeight = value;
                OnPropertyChanged("FrontSuspensionHeight");
            }
        }
        public byte RearSuspensionHeight
        {
            get
            {
                return _rearSuspensionHeight;
            }
            set
            {
                _rearSuspensionHeight = value;
                OnPropertyChanged("RearSuspensionHeight");
            }
        }
        public byte BrakePressure
        {
            get
            {
                return _brakePressure;
            }
            set
            {
                _brakePressure = value;
                OnPropertyChanged("BrakePressure");
            }
        }
        public byte BrakeBias
        {
            get
            {
                return _brakeBias;
            }
            set
            {
                _brakeBias = value;
                OnPropertyChanged("BrakeBias");
            }
        }
        public float RearLeftTyrePressure
        {
            get
            {
                return _rearLeftTyrePressure;
            }
            set
            {
                _rearLeftTyrePressure = value;
                OnPropertyChanged("RearLeftTyrePressure");
            }
        }
        public float RearRightTyrePressure
        {
            get
            {
                return _rearRightTyrePressure;
            }
            set
            {
                _rearRightTyrePressure = value;
                OnPropertyChanged("RearRightTyrePressure");
            }
        }
        public float FrontLeftTyrePressure
        {
            get
            {
                return _frontLeftTyrePressure;
            }
            set
            {
                _frontLeftTyrePressure = value;
                OnPropertyChanged("FrontLeftTyrePressure");
            }
        }
        public float FrontRightTyrePressure
        {
            get
            {
                return _frontRightTyrePressure;
            }
            set
            {
                _frontRightTyrePressure = value;
                OnPropertyChanged("FrontRightTyrePressure");
            }
        }
        public byte Ballast
        {
            get
            {
                return _ballast;
            }
            set
            {
                _ballast = value;
                OnPropertyChanged("Ballast");
            }
        }
        public float FuelLoad
        {
            get
            {
                return _fuelLoad;
            }
            set
            {
                _fuelLoad = value;
                OnPropertyChanged("FuelLoad");
            }
        }

        public override BaseModel Desserialize(byte[] data)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
