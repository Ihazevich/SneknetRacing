using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using SneknetRacing.Commands;
using SneknetRacing.Models;
using SneknetRacing.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace SneknetRacing.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields
        private BaseViewModel _selectedViewModel;

        private PacketHeader _packetHeader;

        private bool _networkThreadsRunning;
        private string _networkButtonStatus;
        private string _networkButtonColor;

        private bool _gamepadConnected;
        private string _gamepadButtonStatus;
        private string _gamepadButtonColor;

        #endregion

        #region Properties
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }

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

        public Server Server { get; }
        public Thread ServerThread { get; }
        public Thread DataHandlerThread { get; }

        public ICommand UpdateViewCommand { get; set; }
        public ICommand StartServerCommand { get; set; }
        public ICommand ConnectGamepadCommand { get; set; }
        public ViGEmClient Client { get; }
        public IXbox360Controller Controller { get; }

        public PacketHeader PacketHeader
        {
            get
            {
                return _packetHeader;
            }
            set
            {
                _packetHeader = value;
                OnPropertyChanged("PacketHeader");
            }
        }
        #endregion

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            StartServerCommand = new StartServerCommand(this);
            ConnectGamepadCommand = new ConnectGamepadCommand(this);

            NetworkThreadsRunning = false;
            GamepadConnected = false;

            Server = new Server();
            ServerThread = new Thread(() => Server.Listen());
            DataHandlerThread = new Thread(() => this.SubscribeToEvent(Server));

            Client = new ViGEmClient();
            Controller = Client.CreateXbox360Controller();

        }
        public void SubscribeToEvent(Server server)
        {
            server.DataReceivedEvent += server_DataReceivedEvent;
        }

        private void server_DataReceivedEvent(object sender, ReceivedDataArgs args)
        {
            SelectedViewModel.Header.Desserialize(args.receivedBytes);
        }

    }
}
