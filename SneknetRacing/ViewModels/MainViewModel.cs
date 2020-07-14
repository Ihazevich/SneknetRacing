using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using SneknetRacing.AI;
using SneknetRacing.Commands;
using SneknetRacing.Models;
using SneknetRacing.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace SneknetRacing.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields
        private BaseViewModel _selectedViewModel;

        private bool _networkThreadsRunning;
        private string _networkButtonStatus;
        private string _networkButtonColor;

        private bool _gamepadConnected;
        private string _gamepadButtonStatus;
        private string _gamepadButtonColor;

        private long _processTime;
        #endregion

        #region Properties
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
                UpdateViewCommand.RaiseCanExecuteChanged();
            }
        }
        public HeaderViewModel HeaderViewModel { get; }
        public MotionDataViewModel MotionDataViewModel { get; }
        public SessionDataViewModel SessionDataViewModel { get; }
        public LapDataViewModel LapDataViewModel { get; }
        public EventDataViewModel EventDataViewModel { get; }
        public ParticipantsDataViewModel ParticipantsDataViewModel { get; }
        public CarSetupsDataViewModel CarSetupsDataViewModel { get; }
        public CarTelemetryDataViewModel CarTelemetryDataViewModel { get; }
        public CarStatusDataViewModel CarStatusDataViewModel { get; }
        public ClassificationDataViewModel ClassificationDataViewModel { get; }
        public LobbyInfoDataViewModel LobbyInfoDataViewModel { get; }
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

        public NeuralInputData NeuralInputData { get; }

        public long ProcessTime
        {
            get
            {
                return _processTime;
            }
            set
            {
                _processTime = value;
                OnPropertyChanged("ProcessTime");
            }
        }

        public Server Server { get; }
        public Thread ServerThread { get; }
        public Thread DataHandlerThread { get; }
        public Thread DataSavingThread { get; }

        public UpdateViewCommand UpdateViewCommand { get; set; }
        public StartServerCommand StartServerCommand { get; set; }
        public ConnectGamepadCommand ConnectGamepadCommand { get; set; }
        public ViGEmClient Client { get; }
        public IXbox360Controller Controller { get; }
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

            HeaderViewModel = new HeaderViewModel();
            MotionDataViewModel = new MotionDataViewModel();
            SessionDataViewModel = new SessionDataViewModel();
            LapDataViewModel = new LapDataViewModel();
            EventDataViewModel = new EventDataViewModel();
            ParticipantsDataViewModel = new ParticipantsDataViewModel();
            CarSetupsDataViewModel = new CarSetupsDataViewModel();
            CarTelemetryDataViewModel = new CarTelemetryDataViewModel();
            CarStatusDataViewModel = new CarStatusDataViewModel();
            ClassificationDataViewModel = new ClassificationDataViewModel();
            LobbyInfoDataViewModel = new LobbyInfoDataViewModel();

            NeuralInputData = new NeuralInputData();

            SelectedViewModel = HeaderViewModel;
            HeaderViewModel.Packet = new PacketHeader()
            {
                PacketID = 1
            };

            MotionDataViewModel.Packet = new PacketMotionData();

            ProcessTime = 0;
        }

        public void SubscribeToEvent(Server server)
        {
            server.DataReceivedEvent += Server_DataReceivedEvent;
        }

        private void Server_DataReceivedEvent(object sender, ReceivedDataArgs args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            HeaderViewModel.Packet.Desserialize(args.receivedBytes);
            var packet = HeaderViewModel.Packet as PacketHeader;

            switch(packet.PacketID)
            {
                case 0:
                    MotionDataViewModel.Packet.Desserialize(args.receivedBytes);
                    NeuralInputData.MotionDataPackets.Add(MotionDataViewModel.Packet as PacketMotionData);
                    break;
                case 1:
                    SessionDataViewModel.Packet.Desserialize(args.receivedBytes);
                    break;
                case 2:
                    LapDataViewModel.Packet.Desserialize(args.receivedBytes);
                    NeuralInputData.LapDataPackets.Add(LapDataViewModel.Packet as PacketLapData);
                    break;
                case 3:
                    EventDataViewModel.Packet.Desserialize(args.receivedBytes);
                    break;
                case 4:
                    ParticipantsDataViewModel.Packet.Desserialize(args.receivedBytes);
                    break;
                case 5:
                    CarSetupsDataViewModel.Packet.Desserialize(args.receivedBytes);
                    break;
                case 6:
                    CarTelemetryDataViewModel.Packet.Desserialize(args.receivedBytes);
                    NeuralInputData.CarTelemetryDataPackets.Add(CarTelemetryDataViewModel.Packet as PacketCarTelemetryData);
                    break;
                case 7:
                    CarStatusDataViewModel.Packet.Desserialize(args.receivedBytes);
                    NeuralInputData.CarStatusDataPackets.Add(CarStatusDataViewModel.Packet as PacketCarStatusData);
                    break;
                case 8:
                    ClassificationDataViewModel.Packet.Desserialize(args.receivedBytes);
                    break;
                case 9:
                    LobbyInfoDataViewModel.Packet.Desserialize(args.receivedBytes);
                    break;
            }

            stopwatch.Stop();
            ProcessTime = stopwatch.ElapsedMilliseconds;
        }

    }
}
