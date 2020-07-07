using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Model
{
    public struct CarMotionDataStruct
    {
        float worldPositionX;           // World space X position
        float worldPositionY;           // World space Y position
        float worldPositionZ;           // World space Z position
        float worldVelocityX;           // Velocity in world space X
        float worldVelocityY;           // Velocity in world space Y
        float worldVelocityZ;           // Velocity in world space Z
        Int16 worldForwardDirX;         // World space forward X direction (normalised)
        Int16 worldForwardDirY;         // World space forward Y direction (normalised)
        Int16 worldForwardDirZ;         // World space forward Z direction (normalised)
        Int16 worldRightDirX;           // World space right X direction (normalised)
        Int16 worldRightDirY;           // World space right Y direction (normalised)
        Int16 worldRightDirZ;           // World space right Z direction (normalised)
        float gForceLateral;            // Lateral G-Force component
        float gForceLongitudinal;       // Longitudinal G-Force component
        float gForceVertical;           // Vertical G-Force component
        float yaw;                      // Yaw angle in radians
        float pitch;                    // Pitch angle in radians
        float roll;                     // Roll angle in radians
    }

    public class CarMotionData : INotifyPropertyChanged
    {
        #region Members

        private float _worldPositionX;           // World space X position
        private float _worldPositionY;           // World space Y position
        private float _worldPositionZ;           // World space Z position
        private float _worldVelocityX;           // Velocity in world space X
        private float _worldVelocityY;           // Velocity in world space Y
        private float _worldVelocityZ;           // Velocity in world space Z
        private Int16 _worldForwardDirX;         // World space forward X direction (normalised)
        private Int16 _worldForwardDirY;         // World space forward Y direction (normalised)
        private Int16 _worldForwardDirZ;         // World space forward Z direction (normalised)
        private Int16 _worldRightDirX;           // World space right X direction (normalised)
        private Int16 _worldRightDirY;           // World space right Y direction (normalised)
        private Int16 _worldRightDirZ;           // World space right Z direction (normalised)
        private float _gForceLateral;            // Lateral G-Force component
        private float _gForceLongitudinal;       // Longitudinal G-Force component
        private float _gForceVertical;           // Vertical G-Force component
        private float _yaw;                      // Yaw angle in radians
        private float _pitch;                    // Pitch angle in radians
        private float _roll;                     // Roll angle in radians

        #endregion

        public float WorldPositionX
        {
            get 
            {
                return _worldPositionX;
            }
            set
            {
                _worldPositionX = value;
                OnPropertyChanged("WorldPositionX");
            }
        }
        public float WorldPositionY
        {
            get
            {
                return _worldPositionY;
            }
            set
            {
                _worldPositionY = value;
                OnPropertyChanged("WorldPositionY");
            }
        }
        public float WorldPositionZ
        {
            get
            {
                return _worldPositionZ;
            }
            set
            {
                _worldPositionZ = value;
                OnPropertyChanged("WorldPositionZ");
            }
        }
        public float WorldVelocityX
        {
            get
            {
                return _worldVelocityX;
            }
            set
            {
                _worldVelocityX = value;
                OnPropertyChanged("WorldVelocityX");
            }
        }
        public float WorldVelocityY
        {
            get
            {
                return _worldVelocityY;
            }
            set
            {
                _worldVelocityY = value;
                OnPropertyChanged("WorldVelocityY");
            }
        }
        public float WorldVelocityZ
        {
            get
            {
                return _worldVelocityZ;
            }
            set
            {
                _worldVelocityZ = value;
                OnPropertyChanged("WorldVelocityZ");
            }
        }
        public Int16 WorldForwardDirX
        {
            get
            {
                return _worldForwardDirX;
            }
            set
            {
                _worldForwardDirX = value;
                OnPropertyChanged("WorldForwardDirX");
            }
        }
        public Int16 WorldForwardDirY
        {
            get
            {
                return _worldForwardDirY;
            }
            set
            {
                _worldForwardDirY = value;
                OnPropertyChanged("WorldForwardDirY");
            }
        }
        public Int16 WorldForwardDirZ
        {
            get
            {
                return _worldForwardDirZ;
            }
            set
            {
                _worldForwardDirZ = value;
                OnPropertyChanged("WorldForwardDirZ");
            }
        }

        public Int16 WorldRightDirX
        {
            get
            {
                return _worldRightDirX;
            }
            set
            {
                _worldRightDirX = value;
                OnPropertyChanged("WorldRightDirX");
            }
        }

        public Int16 WorldRightDirY
        {
            get
            {
                return _worldRightDirY;
            }
            set
            {
                _worldRightDirY = value;
                OnPropertyChanged("WorldRightDirY");
            }
        }
        public Int16 WorldRightDirZ
        {
            get
            {
                return _worldRightDirZ;
            }
            set
            {
                _worldRightDirZ = value;
                OnPropertyChanged("WorldRightDirZ");
            }
        }
        public float GForceLateral
        {
            get
            {
                return _gForceLateral;
            }
            set
            {
                _gForceLateral = value;
                OnPropertyChanged("GForceLateral");
            }
        }
        public float GForceLongitudinal
        {
            get
            {
                return _gForceLongitudinal;
            }
            set
            {
                _gForceLongitudinal = value;
                OnPropertyChanged("GForceLongitudinal");
            }
        }
        public float GForceVertical
        {
            get
            {
                return _gForceVertical;
            }
            set
            {
                _gForceVertical = value;
                OnPropertyChanged("GForceVertical");
            }
        }
        public float Yaw
        {
            get
            {
                return _yaw;
            }
            set
            {
                _yaw = value;
                OnPropertyChanged("Yaw");
            }
        }
        public float Pitch
        {
            get
            {
                return _pitch;
            }
            set
            {
                _pitch = value;
                OnPropertyChanged("Pitch");
            }
        }
        public float Roll
        {
            get
            {
                return _roll;
            }
            set
            {
                _roll = value;
                OnPropertyChanged("Roll");
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
