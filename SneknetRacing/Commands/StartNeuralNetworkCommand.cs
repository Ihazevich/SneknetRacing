using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using SneknetRacing.AI;
using SneknetRacing.ViewModels;


namespace SneknetRacing.Commands
{
    public class StartNeuralNetworkCommand : ICommand
    {
        private bool _isNetworkRunning = false;
        private MainViewModel _viewModel;

        static readonly object _locker = new object();

        public StartNeuralNetworkCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !_isNetworkRunning;
        }

        public void Execute(object parameter)
        {
            if (_isNetworkRunning)
            {

            }
            else
            {
                _isNetworkRunning = true;

                List<float[]> trainingSamples = new List<float[]>();
                List<float[]> expectedValues = new List<float[]>();

                string[] files = Directory.GetFiles("D:\\NeuralData\\0\\HAMILTON");

                List<Task> tasks = new List<Task>();
                ConcurrentQueue<string> readedSamples = new ConcurrentQueue<string>();

                Console.WriteLine("Found {0} files, loading them to memory...", files.Length);
                foreach (string s in files)
                {
                    tasks.Add(Task.Factory.StartNew(() => readedSamples.Enqueue(File.ReadAllText(s))));
                }

                Task.WaitAll(tasks.ToArray());

                Console.WriteLine("Deserializing {0} samples....", readedSamples.Count);
                foreach (string s in readedSamples)
                {
                    RacerSample sample = JsonSerializer.Deserialize<RacerSample>(s);

                    List<float> inputs = new List<float>();
                    List<float> outputs = new List<float>();

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

                List<NeuralNetwork> networks = new List<NeuralNetwork>();
                List<Thread> networkTasks = new List<Thread>();

                ConcurrentStack<NeuralNetwork> bestNetworks = new ConcurrentStack<NeuralNetwork>();
                float bestAccuracy = 0.0f;
                int threads = 13;

                for(int i = 0; i < threads; i++)
                {
                    networks.Add(new NeuralNetwork(trainingSamples[0].Length, expectedValues[0].Length, new int[] { 200, 200, 200, 200, 200, 200, 200, 200 }));
                }

                Parallel.ForEach(networks, network =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        NeuralNetwork temp;
                        if(bestNetworks.TryPeek(out temp))
                        {
                            network = temp;
                            network.Mutate((float)new Random().NextDouble());
                        }
                        else
                        {
                            network.Initialize(new Random());
                        }
                        var accuracy = network.Test(trainingSamples.ToArray(), expectedValues.ToArray());
                        lock (_locker)
                        {
                            if (accuracy > bestAccuracy)
                            {
                                bestAccuracy = accuracy;
                                bestNetworks.Push(network);
                                Console.WriteLine("==================POG BEST SO FAR==================");
                                Console.WriteLine("{0:##.#####}%",accuracy*100);
                                Console.WriteLine("===================================================");
                            }
                        }
                    }
                });

            }

        }
    }
}
