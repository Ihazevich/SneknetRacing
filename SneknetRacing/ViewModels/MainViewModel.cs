using SneknetRacing.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SneknetRacing.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        private bool _networkThreadsRunning;
        private string _networkButtonStatus;
        private string _networkButtonColor;

        private bool _gamepadConnected;
        private string _gamepadButtonStatus;
        private string _gamepadButtonColor;

        public bool NetworkThreadsRunning
        {
            get { return _networkThreadsRunning; }
            set 
            { 
                _networkThreadsRunning = value;
                OnPropertyChanged("NetworkThreadsRunning");
                if(value)
                {
                    NetworkButtonStatus = "UDP Socket listening";
                    NetworkButtonColor = "Green";
                } 
                else
                {
                    NetworkButtonStatus = "Start listening";
                    NetworkButtonColor = "Red";
                }
            }
        }

        public string NetworkButtonStatus
        {
            get
            {
                return _networkButtonStatus;
            }
            set
            {
                _networkButtonStatus = value;
                OnPropertyChanged("NetworkButtonStatus");
            }
        }

        public string NetworkButtonColor
        {
            get
            {
                return _networkButtonColor;
            }
            set
            {
                _networkButtonColor = value;
                OnPropertyChanged("NetworkButtonColor");
            }
        }

        public bool GamepadConnected
        {
            get { return _gamepadConnected; }
            set
            {
                _gamepadConnected = value;
                OnPropertyChanged("GamepadConnected");
                if (value)
                {
                    GamepadButtonStatus = "Gamepad Connected";
                    GamepadButtonColor = "Green";
                }
                else
                {
                    GamepadButtonStatus = "Connect Gamepad";
                    GamepadButtonColor = "Red";
                }
            }
        }

        public string GamepadButtonStatus
        {
            get
            {
                return _gamepadButtonStatus;
            }
            set
            {
                _gamepadButtonStatus = value;
                OnPropertyChanged("GamepadButtonStatus");
            }
        }

        public string GamepadButtonColor
        {
            get
            {
                return _gamepadButtonColor;
            }
            set
            {
                _gamepadButtonColor = value;
                OnPropertyChanged("GamepadButtonColor");
            }
        }
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            { 
                _selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }

        public ICommand UpdateViewCommand { get; set; } 

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            NetworkThreadsRunning = false;
            GamepadConnected = false;
        }



    }
}
