using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using SneknetRacing.AI;
using SneknetRacing.Commands;
using SneknetRacing.Log;
using SneknetRacing.Models;
using SneknetRacing.Network;
using SneknetRacing.Views;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace SneknetRacing.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields
        private BaseViewModel _selectedViewModel;
        private ConcurrentQueue<byte[]> _receivedRawPackets = new ConcurrentQueue<byte[]>();

        static readonly object _locker = new object();

        private bool _networkThreadsRunning;
        private string _networkButtonContent;
        private Visibility _networkThreadsVisibility = Visibility.Hidden;

        private bool _gamepadConnected;
        private string _gamepadButtonContent;
        private Visibility _gamepadVisibility = Visibility.Hidden;

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
        public ConsoleViewModel ConsoleViewModel { get; }

        public bool NetworkThreadsRunning
        {
            get { return _networkThreadsRunning; }
            set
            {
                _networkThreadsRunning = value;
                OnPropertyChanged("NetworkThreadsRunning");
                if (value)
                {
                    NetworkButtonContent = "Stop UDP Listener";
                    NetworkThreadsVisibility = Visibility.Visible;
                }
                else
                {
                    NetworkButtonContent = "Start UDP Listener";
                    NetworkThreadsVisibility = Visibility.Hidden;
                }
                StartServerCommand.RaiseCanExecuteChanged();
            }
        }

        public string NetworkButtonContent
        {
            get
            {
                return _networkButtonContent;
            }
            set
            {
                _networkButtonContent = value;
                OnPropertyChanged("NetworkButtonStatus");
            }
        }

        public Visibility NetworkThreadsVisibility
        {
            get
            {
                return _networkThreadsVisibility;
            }
            set
            {
                _networkThreadsVisibility = value;
                OnPropertyChanged(nameof(NetworkThreadsVisibility));
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
                    GamepadButtonContent = "Gamepad Connected";
                    GamepadVisibility = Visibility.Visible;
                }
                else
                {
                    GamepadButtonContent = "Connect Gamepad";
                    GamepadVisibility = Visibility.Hidden;
                }
            }
        }

        public string GamepadButtonContent
        {
            get
            {
                return _gamepadButtonContent;
            }
            set
            {
                _gamepadButtonContent = value;
                OnPropertyChanged("GamepadButtonStatus");
            }
        }

        public Visibility GamepadVisibility
        {
            get
            {
                return _gamepadVisibility;
            }
            set
            {
                _gamepadVisibility = value;
                OnPropertyChanged(nameof(GamepadVisibility));
            }
        }

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
        public StartNeuralNetworkCommand StartNeuralNetworkCommand { get; set; }

        public Logger Logger { get; set; }

        public ViGEmClient Client { get; }
        public IXbox360Controller Controller { get; }
        #endregion

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            StartServerCommand = new StartServerCommand(this);
            ConnectGamepadCommand = new ConnectGamepadCommand(this);
            UpdateMotionViewCommand = new UpdateMotionViewCommand(this);
            StartNeuralNetworkCommand = new StartNeuralNetworkCommand(this);

            NetworkThreadsRunning = false;
            GamepadConnected = false;

            Logger = new Logger();

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

            ConsoleViewModel = new ConsoleViewModel(Logger);

            SelectedViewModel = ConsoleViewModel;

            ProcessTime = 0;

            Server = new Server();
            
            ServerThread = new Task(() => Server.Listen());
            DataHandlerThread = new Task(() => SubscribeToServerEvent(Server));
            DesserializationThread = new Task(() => Desserialize());
            //SerializerThread = new Task(() => SerializeNeuralInputs());
            SerializerThread = new Task(() => GenerateNeuralDataModel());

            BindingOperations.EnableCollectionSynchronization(Logger.LogEntries, _locker);
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
                    Directory.CreateDirectory("D:\\NeuralData\\" + track);
                    while (true)
                    {
                        if (ParticipantsDataViewModel.ProcessedPackets.TryPeek(out participantsPacket))
                        {
                            for (int i = 0; i < participantsPacket.NumActiveCars; i++)
                            {
                                drivers.Add(participantsPacket.Participants[i].Name);
                                Directory.CreateDirectory("D:\\NeuralData\\" + track + "\\" + drivers[i]);
                            }
                            break;
                        }
                    }
                    break;
                }
            }

            while (true)
            {
                bool foundTelemetry = false;
                bool foundMotion = false;
                bool foundLap = false;
                bool foundStatus = false;

                if ( CarStatusDataViewModel.ProcessedPackets.TryPeek(out _) &&
                     CarTelemetryDataViewModel.ProcessedPackets.TryPeek(out _) &&
                     MotionDataViewModel.ProcessedPackets.TryPeek(out _) &&
                     LapDataViewModel.ProcessedPackets.TryPeek(out _))
                {
                    // If they exist, dequeue them and assign them to their correspoonding variables
                    // CarStatusDataViewModel.ProcessedPackets.TryDequeue(out carStatusPacket);
                    while (!foundStatus)
                    {
                        foundStatus = CarStatusDataViewModel.ProcessedPackets.TryDequeue(out carStatusPacket);
                    }
                    while (!foundTelemetry)
                    {
                        foundTelemetry = CarTelemetryDataViewModel.ProcessedPackets.TryDequeue(out carTelemetryPacket);
                    }
                    while(!foundMotion)
                    {
                        foundMotion = MotionDataViewModel.ProcessedPackets.TryDequeue(out motionPacket);
                    }
                    while(!foundLap)
                    {
                        foundLap = LapDataViewModel.ProcessedPackets.TryDequeue(out lapPacket);
                    }                    

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
                        Console.WriteLine(participantsPacket.Participants[i].Name);

                        if(participantsPacket.Participants[i].Name == "HAMILTON")
                        {
                            Console.WriteLine("Creating HAMILTON sample");
                            RacerSample sample = new RacerSample();

                            sample.Speed = carTelemetryPacket.CarTelemetryData[i].Speed;
                            sample.CurrentGear = carTelemetryPacket.CarTelemetryData[i].Gear / (float)carStatusPacket.CarStatusData[i].MaxGears;
                            sample.EngineRPM = (float)carTelemetryPacket.CarTelemetryData[i].EngineRPM / (float)carStatusPacket.CarStatusData[i].MaxRPM;
                            sample.SurfaceTypeRL = carTelemetryPacket.CarTelemetryData[i].SurfaceType[0];
                            sample.SurfaceTypeRR = carTelemetryPacket.CarTelemetryData[i].SurfaceType[1];
                            sample.SurfaceTypeFL = carTelemetryPacket.CarTelemetryData[i].SurfaceType[2];
                            sample.SurfaceTypeFR = carTelemetryPacket.CarTelemetryData[i].SurfaceType[3];
                            sample.LapDistance = lapPacket.LapData[i].LapDistance / sessionPacket.TrackLength;
                            sample.WorldPosX = motionPacket.CarMotionData[i].WorldPositionX;
                            sample.WorldPosZ = motionPacket.CarMotionData[i].WorldPositionZ;
                            /*
                            sample.WorldForwardDirX = motionPacket.CarMotionData[i].WorldForwardDirX;
                            sample.WorldForwardDirZ = motionPacket.CarMotionData[i].WorldForwardDirZ;
                            sample.WorldRightDirX = motionPacket.CarMotionData[i].WorldRightDirX;
                            sample.WorldRightDirZ = motionPacket.CarMotionData[i].WorldRightDirZ;
                            sample.Yaw = motionPacket.CarMotionData[i].Yaw / (Math.PI * 2);
                            sample.Pitch = motionPacket.CarMotionData[i].Pitch / (Math.PI * 2);
                            sample.Roll = motionPacket.CarMotionData[i].Roll / (Math.PI * 2);
                            */
                            sample.Throttle = carTelemetryPacket.CarTelemetryData[i].Throttle - carTelemetryPacket.CarTelemetryData[i].Brake;
                            sample.Steer = carTelemetryPacket.CarTelemetryData[i].Steer;

                            string path = "D:\\NeuralData\\" + sessionPacket.TrackID + "\\" + participantsPacket.Participants[i].Name + "\\" + DateTime.Now.Ticks + ".json";

                            var options = new JsonSerializerOptions
                            {
                                WriteIndented = true,
                            };

                            string jsonString = JsonSerializer.Serialize(sample, options);
                            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\NeuralData\\" + DateTime.Now.Ticks + ".json", jsonString);
                            // File.WriteAllText(path, jsonString);
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

        public void GenerateNetworks()
        {
            List<double[]> trainingSamples = new List<double[]>();
            List<double[]> expectedValues = new List<double[]>();

            string[] files = Directory.GetFiles("D:\\NeuralData\\0\\HAMILTON");

            List<Task> tasks = new List<Task>();
            ConcurrentQueue<string> readedSamples = new ConcurrentQueue<string>();

            Logger.Log(String.Format("Found {0} files, loading them to memory...", files.Length));
            foreach (string s in files)
            {
                tasks.Add(Task.Factory.StartNew(() => readedSamples.Enqueue(File.ReadAllText(s)),CancellationToken.None,TaskCreationOptions.PreferFairness,TaskScheduler.Default));
            }

            Task.WaitAll(tasks.ToArray());

            Logger.Log(String.Format("Deserializing {0} samples....", readedSamples.Count));
            foreach (string s in readedSamples)
            {
                RacerSample sample = JsonSerializer.Deserialize<RacerSample>(s);

                List<double> inputs = new List<double>();
                List<double> outputs = new List<double>();

                inputs.Add(sample.Speed);
                inputs.Add(sample.CurrentGear);
                inputs.Add(sample.EngineRPM);
                inputs.Add(sample.SurfaceTypeRL);
                inputs.Add(sample.SurfaceTypeRR);
                inputs.Add(sample.SurfaceTypeFL);
                inputs.Add(sample.SurfaceTypeFR);
                inputs.Add(sample.LapDistance);
                inputs.Add(sample.WorldPosX);
                inputs.Add(sample.WorldPosZ);
                /*
                inputs.Add(sample.WorldForwardDirX);
                inputs.Add(sample.WorldForwardDirZ);
                inputs.Add(sample.WorldRightDirX);
                inputs.Add(sample.WorldRightDirZ);
                inputs.Add(sample.Yaw);
                inputs.Add(sample.Pitch);
                inputs.Add(sample.Roll);
                */
                outputs.Add(sample.Throttle);
                outputs.Add(sample.Steer);
                outputs.Add(sample.CurrentGear);

                trainingSamples.Add(inputs.ToArray());
                expectedValues.Add(outputs.ToArray());
            }

            Console.WriteLine("Creating network");
            Logger.Log("Creating network");
            List<NeuralNetwork> networks = new List<NeuralNetwork>();

            NeuralNetwork bestNetwork = null;
            double bestFitness = 9999999.0;
            int concurrentNetworks = 10;

            for (int i = 0; i < concurrentNetworks; i++)
            {
                networks.Add(new NeuralNetwork(trainingSamples[0].Length, expectedValues[0].Length, new int[] { 500 }));
            }

            Stopwatch totalTime = Stopwatch.StartNew();
            Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.ForEach(networks, network =>
            {
                int eppoch = 1;
                while (true)
                {
                    var best = false;
                    lock (_locker)
                    {
                        if (bestNetwork != null)
                        {
                            network = new NeuralNetwork(bestNetwork.GetWeights(), bestNetwork.GetNodesStatus(), trainingSamples[0].Length, bestNetwork.Fitness);
                            //Console.WriteLine("mutatooooo");
                            network.Mutate(bestFitness);
                        }
                        else
                        {
                            /*string[] files = Directory.GetFiles("D:\\NeuralData\\0\\HAMILTON");
                            double[][][] networkWeights;
                            if (files.Length > 0)
                            {
                                Console.WriteLine("Loading Network " + files[0]);
                                string jsonString = File.ReadAllText(files[0]);
                                networkWeights = JsonSerializer.Deserialize<double[][][]>(jsonString);
                                network = new NeuralNetwork(networkWeights, trainingSamples[0].Length);
                            }
                            else
                            {

                            }*/
                            network.Initialize();
                        }
                    }
                    network.Test(trainingSamples.ToArray(), expectedValues.ToArray());

                    var fitness = network.Fitness;

                    lock (_locker)
                    {
                        Logger.Log(String.Format("Thread: {0} | Eppoch: {1} | Error: {2} | Total time: {3}",networks.IndexOf(network) + 1, eppoch + 1, fitness, totalTime.Elapsed.ToString()));
                        if (fitness < bestFitness)
                        {
                            stopwatch.Stop();
                            best = true;
                            bestFitness = fitness;
                            bestNetwork = new NeuralNetwork(network.GetWeights(), network.GetNodesStatus(), trainingSamples[0].Length, network.Fitness);
                            //Console.WriteLine("==================POG BEST SO FAR==================");
                            Logger.Log(String.Format("NEW BEST! Error: {0} | Time since last best: {1}", fitness, stopwatch.Elapsed.ToString()));
                            //Console.WriteLine("===================================================");
                            stopwatch.Restart();
                        }
                    }
                    if (best)
                    {
                        Directory.CreateDirectory("D:\\NeuralData\\0\\HAMILTON\\Networks\\");
                        string path = "D:\\NeuralData\\0\\HAMILTON\\Networks\\" + fitness + ".json";
                        Console.WriteLine(path);
                        var options = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                        };

                        string jsonString = JsonSerializer.Serialize(network.GetWeights(), options);
                        File.WriteAllText(path, jsonString);
                    }
                    eppoch++;

                }
            });
        }
    }
}
