﻿using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using SneknetRacing.AI;
using SneknetRacing.Commands;
using SneknetRacing.Models;
using SneknetRacing.Network;
using SneknetRacing.Views;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SneknetRacing.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields
        private BaseViewModel _selectedViewModel;
        private ConcurrentQueue<byte[]> _receivedRawPackets = new ConcurrentQueue<byte[]>();

        private bool _networkThreadsRunning;
        private string _networkButtonStatus;
        private string _networkButtonColor;

        private bool _gamepadConnected;
        private string _gamepadButtonStatus;
        private string _gamepadButtonColor;

        private long _processTime;
        #endregion

        #region Properties
        public ConcurrentQueue<byte[]> ReceivedPackets
        {
            get
            {
                return _receivedRawPackets;
            }
            set 
            {
                _receivedRawPackets = value;
                OnPropertyChanged("ReceivedPackets");
            } 
        }
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

        public NeuralInputData NeuralInputData { get; set; }

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
        public Task SerializerThread { get; }

        public UpdateViewCommand UpdateViewCommand { get; set; }
        public StartServerCommand StartServerCommand { get; set; }
        public ConnectGamepadCommand ConnectGamepadCommand { get; set; }
        public UpdateMotionViewCommand UpdateMotionViewCommand { get; set; }
        public ViGEmClient Client { get; }
        public IXbox360Controller Controller { get; }
        #endregion

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            StartServerCommand = new StartServerCommand(this);
            ConnectGamepadCommand = new ConnectGamepadCommand(this);
            UpdateMotionViewCommand = new UpdateMotionViewCommand(this);

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
            DataHandlerThread = new Task(() => SubscribeToServerEvent(Server));
            DesserializationThread = Task.Factory.StartNew(() => Desserialize());
            //SerializerThread = new Task(() => SerializeNeuralInputs());
            SerializerThread = new Task(() => GenerateNeuralDataModel());
        }

        public void SubscribeToServerEvent(Server server)
        {
            server.DataReceivedEvent += Server_DataReceivedEvent;
        }

        private void Server_DataReceivedEvent(object sender, ReceivedDataArgs args)
        {
            AddPacketToDesserializationQueue(args.receivedBytes);            
        }

        public void Desserialize()
        {
            Console.WriteLine(this + " DesserializationThread started...");
            while (true)
            {
                byte[] rawPacket;
                if (ReceivedPackets.TryDequeue(out rawPacket))
                {
                    HeaderViewModel.Packet = HeaderViewModel.Packet.Desserialize(rawPacket) as PacketHeader;
                    Console.Write(HeaderViewModel.Packet.PacketID);

                    switch (HeaderViewModel.Packet.PacketID)
                    {
                        case 0:
                            MotionDataViewModel.AddPacketToDesserializationQueue(rawPacket);
                            //Task motionTask2 = motionTask1.ContinueWith(ant => NeuralInputData.MotionDataPackets.Add(MotionDataViewModel.Packet as PacketMotionData));
                            break;
                        case 1:
                            SessionDataViewModel.AddPacketToDesserializationQueue(rawPacket);
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

        public void SerializeNeuralInputs()
        {
            PacketCarSetupData carSetupData = null;
            PacketCarStatusData carStatusData = null;
            PacketCarTelemetryData carTelemetryData = null;
            //PacketEventData packetEventData;
            //PacketFinalClassificationData packetFinalClassificationData;
            PacketLapData lapData = null;
            PacketMotionData motionData = null;
            PacketParticipantsData participantsData = null;
            PacketSessionData sessionData = null;

            while(true)
            {
                // First the 60hz packets

                // Check if all 4 main packets exist
                if (CarStatusDataViewModel.ProcessedPackets.TryPeek(out carStatusData) &&
                    CarTelemetryDataViewModel.ProcessedPackets.TryPeek(out carTelemetryData) &&
                    MotionDataViewModel.ProcessedPackets.TryPeek(out motionData) &&
                    LapDataViewModel.ProcessedPackets.TryPeek(out lapData))
                {
                    // If they exist, dequeue them and assign them to their correspoonding variables
                    CarStatusDataViewModel.ProcessedPackets.TryDequeue(out carStatusData);
                    CarTelemetryDataViewModel.ProcessedPackets.TryDequeue(out carTelemetryData);
                    MotionDataViewModel.ProcessedPackets.TryDequeue(out motionData);
                    LapDataViewModel.ProcessedPackets.TryDequeue(out lapData);

                    // Check if car status packet was ever retrieved
                    if (carSetupData != null)
                    {
                        // If it was, check if there is a new packet
                        if (CarSetupsDataViewModel.ProcessedPackets.TryPeek(out PacketCarSetupData temp))
                        {
                            // Check if the session time in the new packet is less than the session time in our most recent motion packet
                            if (temp.Header.SessionTime <= motionData.Header.SessionTime)
                            {
                                // If it is, replace car setup packet with the new one
                                CarSetupsDataViewModel.ProcessedPackets.TryDequeue(out carSetupData);
                            }
                        }
                    }
                    else
                    {
                        // If a car status packet was never retrieved, wait until one gets to the queue and retrieve it
                        while (!CarSetupsDataViewModel.ProcessedPackets.TryDequeue(out carSetupData))
                        {
                        };
                    }

                    // Check if session packet was ever retrieved
                    if (sessionData != null)
                    {
                        // If it was, check if there is a new packet
                        if (SessionDataViewModel.ProcessedPackets.TryPeek(out PacketSessionData temp))
                        {
                            // Check if the session time in the new packet is less than the session time in our most recent motion packet
                            if (temp.Header.SessionTime <= motionData.Header.SessionTime)
                            {
                                // If it is, replace session packet with the new one
                                SessionDataViewModel.ProcessedPackets.TryDequeue(out sessionData);
                            }
                        }
                    }
                    else
                    {
                        // If a session packet was never retrieved, wait until one gets to the queue and retrieve it
                        while (!SessionDataViewModel.ProcessedPackets.TryDequeue(out sessionData))
                        {
                        };
                    }

                    // Check if participants packet was ever retrieved
                    if (participantsData != null)
                    {
                        // If it was, check if there is a new packet
                        if (ParticipantsDataViewModel.ProcessedPackets.TryPeek(out PacketParticipantsData temp))
                        {
                            // Check if the session time in the new packet is less than the session time in our most recent motion packet
                            if (temp.Header.SessionTime <= motionData.Header.SessionTime)
                            {
                                // If it is, replace session packet with the new one
                                ParticipantsDataViewModel.ProcessedPackets.TryDequeue(out participantsData);
                            }
                        }
                    }
                    else
                    {
                        // If a session packet was never retrieved, wait until one gets to the queue and retrieve it
                        while (!ParticipantsDataViewModel.ProcessedPackets.TryDequeue(out participantsData))
                        {
                        };
                    }

                    // When we have all packets, assign them to NeuralInputData
                    NeuralInputData = new NeuralInputData()
                    {
                        CarSetupData = carSetupData,
                        CarStatusData = carStatusData,
                        CarTelemetryData = carTelemetryData,
                        MotionData = motionData,
                        LapData = lapData,
                        ParticipantsData = participantsData,
                        SessionData = sessionData
                    };

                    // Serialize NeuralInputData to a JSON file, using the current timestamp as the file name
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    };
                    string jsonString = JsonSerializer.Serialize(NeuralInputData, options);
                    //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\NeuralData\\" + DateTime.Now.Ticks + ".json", jsonString);
                    File.WriteAllText("D:\\NeuralData\\" + DateTime.Now.Ticks + ".json", jsonString);
                }               
            }
        }

        public void GenerateNeuralDataModel()
        {
            Console.WriteLine("Starting NeuralData saving thread...");
            PacketCarSetupData carSetupPacket = null;
            PacketCarStatusData carStatusPacket = null;
            PacketCarTelemetryData carTelemetryPacket = null;
            //PacketEventData packetEventData;
            //PacketFinalClassificationData packetFinalClassificationData;
            PacketLapData lapPacket = null;
            PacketMotionData motionPacket = null;
            PacketParticipantsData participantsPacket = null;
            PacketSessionData sessionPacket = null;

            // Grab track and drivers and create directory tree
            int track = -1;
            List<string> drivers = new List<string>();
            while (true)
            {
                if (SessionDataViewModel.ProcessedPackets.TryPeek(out sessionPacket))
                {
                    track = sessionPacket.TrackID;
                    Directory.CreateDirectory("C:\\NeuralData\\" + track);
                    while (true)
                    {
                        if (ParticipantsDataViewModel.ProcessedPackets.TryPeek(out participantsPacket))
                        {
                            for (int i = 0; i < participantsPacket.NumActiveCars; i++)
                            {
                                drivers.Add(participantsPacket.Participants[i].Name);
                                Directory.CreateDirectory("C:\\NeuralData\\" + track + "\\" + drivers[i]);
                            }
                            break;
                        }
                    }
                    break;
                }
            }

            while (true)
            {
                Console.Write("Waiting for packets");
                if (CarStatusDataViewModel.ProcessedPackets.TryPeek(out _) &&
                        CarTelemetryDataViewModel.ProcessedPackets.TryPeek(out _) &&
                        MotionDataViewModel.ProcessedPackets.TryPeek(out _) &&
                        LapDataViewModel.ProcessedPackets.TryPeek(out _))
                {
                    // If they exist, dequeue them and assign them to their correspoonding variables
                    CarStatusDataViewModel.ProcessedPackets.TryDequeue(out carStatusPacket);
                    CarTelemetryDataViewModel.ProcessedPackets.TryDequeue(out carTelemetryPacket);
                    MotionDataViewModel.ProcessedPackets.TryDequeue(out motionPacket);
                    LapDataViewModel.ProcessedPackets.TryDequeue(out lapPacket);

                    Console.WriteLine("Found, Processing");
                    // Check if car setup packet was ever retrieved
                    if (carSetupPacket != null)
                    {
                        // If it was, check if there is a new packet
                        if (CarSetupsDataViewModel.ProcessedPackets.TryPeek(out PacketCarSetupData newSetupPacket))
                        {
                            // Check if the session time in the new packet is less than the session time in our most recent motion packet
                            if (newSetupPacket.Header.SessionTime <= motionPacket.Header.SessionTime)
                            {
                                // If it is, replace car setup packet with the new one
                                CarSetupsDataViewModel.ProcessedPackets.TryDequeue(out carSetupPacket);
                            }
                        }
                    }
                    else
                    {
                        // If a car status packet was never retrieved, wait until one gets to the queue and retrieve it
                        while (!CarSetupsDataViewModel.ProcessedPackets.TryDequeue(out carSetupPacket))
                        {
                        };
                    }

                    // Check if there is a new participants packet
                    if (ParticipantsDataViewModel.ProcessedPackets.TryPeek(out PacketParticipantsData newParticipantPacket))
                    {
                        // Check if the session time in the new packet is less than the session time in our most recent motion packet
                        if (newParticipantPacket.Header.SessionTime <= motionPacket.Header.SessionTime)
                        {
                            // If it is, replace session packet with the new one
                            ParticipantsDataViewModel.ProcessedPackets.TryDequeue(out participantsPacket);
                        }
                    }

                    // When we have all packets, create NeuralDataModel for each driver and save it
                    for(int i = 0; i < participantsPacket.NumActiveCars; i++)
                    {
                        /*
                        NeuralDataModel dataModel = new NeuralDataModel();
                        dataModel.TelemetryData = carTelemetryPacket.CarTelemetryData[i];
                        dataModel.MotionData = motionPacket.CarMotionData[i];
                        dataModel.LapData = lapPacket.LapData[i];
                        dataModel.SetupData = carSetupPacket.CarSetups[i];
                        dataModel.StatusData = carStatusPacket.CarStatusData[i];
                        */
                        Console.WriteLine(participantsPacket.Participants[i].Name);

                        if(participantsPacket.Participants[i].Name == "HAMILTON")
                        {
                            Console.WriteLine("Creating HAMILTON sample");
                            RacerSample sample = new RacerSample();

                            sample.Speed = carTelemetryPacket.CarTelemetryData[i].Speed;
                            sample.CurrentGear = carTelemetryPacket.CarTelemetryData[i].Gear;
                            sample.EngineRPM = carTelemetryPacket.CarTelemetryData[i].EngineRPM;
                            sample.SurfaceTypeRL = carTelemetryPacket.CarTelemetryData[i].SurfaceType[0];
                            sample.SurfaceTypeRR = carTelemetryPacket.CarTelemetryData[i].SurfaceType[1];
                            sample.SurfaceTypeFL = carTelemetryPacket.CarTelemetryData[i].SurfaceType[2];
                            sample.SurfaceTypeFR = carTelemetryPacket.CarTelemetryData[i].SurfaceType[3];
                            sample.LapDistance = lapPacket.LapData[i].LapDistance;
                            sample.WorldPosX = motionPacket.CarMotionData[i].WorldPositionX;
                            sample.WorldPosZ = motionPacket.CarMotionData[i].WorldPositionZ;
                            sample.WorldForwardDirX = motionPacket.CarMotionData[i].WorldForwardDirX;
                            sample.WorldForwardDirZ = motionPacket.CarMotionData[i].WorldForwardDirZ;
                            sample.WorldRightDirX = motionPacket.CarMotionData[i].WorldRightDirX;
                            sample.WorldRightDirZ = motionPacket.CarMotionData[i].WorldRightDirZ;
                            sample.Yaw = motionPacket.CarMotionData[i].Yaw;
                            sample.Pitch = motionPacket.CarMotionData[i].Pitch;
                            sample.Roll = motionPacket.CarMotionData[i].Roll;

                            sample.Throttle = carTelemetryPacket.CarTelemetryData[i].Throttle - carTelemetryPacket.CarTelemetryData[i].Brake;
                            sample.Steer = carTelemetryPacket.CarTelemetryData[i].Steer;

                            string path = "C:\\NeuralData\\" + sessionPacket.TrackID + "\\" + participantsPacket.Participants[i].Name + "\\" + DateTime.Now.Ticks + ".json";

                            Console.WriteLine(DateTime.Now.Ticks);
                            Task.Factory.StartNew(() => SaveToJSON(sample, path));
                        }
                    }
                }
            }
        }

        public void SaveToJSON(RacerSample sample, string path)
        {
            Console.WriteLine("Savinhg file");
            // Serialize the model to a JSON file, using the current timestamp as the file name
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string jsonString = JsonSerializer.Serialize(sample, options);
            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\NeuralData\\" + DateTime.Now.Ticks + ".json", jsonString);
            File.WriteAllText(path, jsonString);
        }

        public void SaveToCSV(RacerSample sample, string path)
        {
            string data = "";

            data += sample.Speed + ",";
            data += sample.EngineRPM + ",";
            data += sample.SurfaceTypeRL + ",";
            data += sample.SurfaceTypeRR + ",";
            data += sample.SurfaceTypeFL + ",";
            data += sample.SurfaceTypeFR + ",";
            data += sample.LapDistance + ",";
            data += sample.WorldPosX + ",";
            data += sample.WorldPosZ + ",";
            data += sample.WorldForwardDirX + ",";
            data += sample.WorldForwardDirZ + ",";
            data += sample.WorldRightDirX + ",";
            data += sample.WorldRightDirZ + ",";
            data += sample.Yaw + ",";
            data += sample.Pitch + ",";
            data += sample.Roll + ",";

            data += sample.CurrentGear + ",";

            File.AppendAllText(path, data);
        }

        public bool AddPacketToDesserializationQueue(byte[] data)
        {
            try
            {
                ReceivedPackets.Enqueue(data);
                TotalPackets++;
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally
            {
            }
        }
    }
}
