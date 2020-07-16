using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using SneknetRacing.AI;
using SneknetRacing.Commands;
using SneknetRacing.Models;
using SneknetRacing.Network;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        public NeuralDataViewModel NeuralDataViewModel { get; }
        public bool NetworkThreadsRunning
        {
            get { return _networkThreadsRunning; }
            set
            {
                _networkThreadsRunning = value;
                OnPropertyChanged("NetworkThreadsRunning");
                if (value)
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
        public Task ServerThread { get; }
        public Task DataHandlerThread { get; }
        public Task DataSavingThread { get; }
        public Task DesserializationThread { get; }

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

            NeuralDataViewModel = new NeuralDataViewModel();
            NeuralInputData = new NeuralInputData();

            SelectedViewModel = HeaderViewModel;

            ProcessTime = 0;

            Server = new Server();
            
            ServerThread = new Task(() => Server.Listen());
            DataHandlerThread = new Task(() => SubscribeToEvent(Server));
        }

        public void SubscribeToEvent(Server server)
        {
            server.DataReceivedEvent += Server_DataReceivedEvent;
        }

        private void Server_DataReceivedEvent(object sender, ReceivedDataArgs args)
        {
            AddPacketToDesserializationQueue(args.receivedBytes);            
        }

        public override void Desserialize()
        {
            while (true)
            {
                byte[] rawPacket;
                if (ReceivedPackets.TryDequeue(out rawPacket))
                {
                    HeaderViewModel.Packet = HeaderViewModel.Packet.Desserialize(rawPacket);
                    var header = HeaderViewModel.Packet as PacketHeader;

                    switch (header.PacketID)
                    {
                        case 0:
                            Packet = MotionDataViewModel.Packet.Desserialize(rawPacket);
                            //Task motionTask2 = motionTask1.ContinueWith(ant => NeuralInputData.MotionDataPackets.Add(MotionDataViewModel.Packet as PacketMotionData));
                            break;
                        case 1:
                            Packet = SessionDataViewModel.Packet.Desserialize(rawPacket);
                            break;
                        case 2:
                            LapDataViewModel.AddPacketToDesserializationQueue(rawPacket);
                            //Task lapTask2 = lapTask1.ContinueWith(ant => NeuralInputData.LapDataPackets.Add(LapDataViewModel.Packet as PacketLapData));
                            break;
                        case 3:
                            EventDataViewModel.AddPacketToDesserializationQueue(rawPacket);
                            break;
                        case 4:
                            ParticipantsDataViewModel.AddPacketToDesserializationQueue(rawPacket);
                            break;
                        case 5:
                            CarSetupsDataViewModel.AddPacketToDesserializationQueue(rawPacket);
                            break;
                        case 6:
                            CarTelemetryDataViewModel.AddPacketToDesserializationQueue(rawPacket);
                            //Task telemetryTask2 = telemetryTask1.ContinueWith(ant => NeuralInputData.CarTelemetryDataPackets.Add(CarTelemetryDataViewModel.Packet as PacketCarTelemetryData));
                            break;
                        case 7:
                            CarStatusDataViewModel.AddPacketToDesserializationQueue(rawPacket);
                            //Task statusTask2 = statusTask1.ContinueWith(ant => NeuralInputData.CarStatusDataPackets.Add(CarStatusDataViewModel.Packet as PacketCarStatusData));
                            break;
                        case 8:
                            ClassificationDataViewModel.AddPacketToDesserializationQueue(rawPacket);
                            break;
                        case 9:
                            LobbyInfoDataViewModel.AddPacketToDesserializationQueue(rawPacket);
                            break;
                    }
                }
            }
        }

    }
}
