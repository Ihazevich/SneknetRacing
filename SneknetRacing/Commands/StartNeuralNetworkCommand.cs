﻿using System;
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
using MathNet.Numerics;
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

                List<double[]> trainingSamples = new List<double[]>();
                List<double[]> expectedValues = new List<double[]>();

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

                List<NeuralNetwork> networks = new List<NeuralNetwork>();
                List<Thread> networkTasks = new List<Thread>();

                NeuralNetwork bestNetwork = null;
                double bestFitness = 1.0;
                int concurrentNetworks = 15;

                for(int i = 0; i < concurrentNetworks; i++)
                {
                    networks.Add(new NeuralNetwork(trainingSamples[0].Length, expectedValues[0].Length, new int[] { 500, 500, 500, 500 }));
                }

                Stopwatch totalTime = Stopwatch.StartNew();
                Stopwatch stopwatch = Stopwatch.StartNew();

                Parallel.ForEach(networks, network =>
                {
                    int eppoch = 1;
                    while(true)
                    {
                        var best = false;
                        lock (_locker)
                        {
                            if (bestNetwork != null)
                            {
                                network = new NeuralNetwork(bestNetwork.GetWeights(), trainingSamples[0].Length);
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
                            Console.WriteLine("Thread: {0} | Eppoch: {1} | Error: {2} | Total time: {3}",
                                networks.IndexOf(network) + 1, eppoch + 1, fitness, totalTime.Elapsed.ToString());
                            if (fitness < bestFitness)
                            {
                                stopwatch.Stop();
                                best = true;
                                bestFitness = fitness;
                                bestNetwork = new NeuralNetwork(network.GetWeights(), trainingSamples[0].Length);
                                Console.WriteLine("==================POG BEST SO FAR==================");
                                Console.WriteLine("Error: {0} | Time since last best: {1}", fitness, stopwatch.Elapsed.ToString());
                                Console.WriteLine("===================================================");
                                stopwatch.Restart();
                            }
                        }
                        if(best)
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
}
