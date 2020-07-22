using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
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

                NeuralNetwork network = new NeuralNetwork(trainingSamples[0].Length, expectedValues[0].Length, new int[] { 200, 200, 200, 200, 200, 200, 200, 200 });
                network.Initialize(new Random(0));
                Stopwatch stopwatch1 = Stopwatch.StartNew();
                Stopwatch stopwatch2 = new Stopwatch();

                float[][] output = new float[trainingSamples.Count][];
                float totalError = 0;
                float accuracy = 0;

                for (int i = 0; i < trainingSamples.Count; i++)
                {
                    /*Console.Write("Inputs:");
                    foreach(var input in trainingSamples[i])
                    {
                        Console.Write("{0}, ", input);
                    }
                    Console.WriteLine();*/
                    stopwatch2.Restart();
                    output[i] = network.Process(trainingSamples[i]);
                    for (int j = 0; j < output[0].Length; j++)
                    {
                        float err = output[i][j] - expectedValues[i][j];
                        totalError += (err * err);
                    }
                    stopwatch2.Stop();
                    //Console.WriteLine("Sample {0} processed in {1}ms. O: {2}, {3}, {4} | E: {5}, {6}, {7}",
                    //i+1, stopwatch2.ElapsedMilliseconds, output[0], output[1], output[2], expectedValues[i][0], expectedValues[i][1], expectedValues[i][2]);
                }
                totalError = totalError / (float)(trainingSamples.Count * trainingSamples[0].Length);

                stopwatch2.Stop();
                Console.WriteLine("Processed {0} samples in {1}ms. Error: {2}", trainingSamples.Count, stopwatch1.ElapsedMilliseconds, totalError);

            }
        }
    }
}
