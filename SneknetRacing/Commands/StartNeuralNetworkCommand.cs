using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.ML.Calibrators;
using SneknetRacing.AI;
using SneknetRacing.ViewModels;


namespace SneknetRacing.Commands
{
    class StartNeuralNetworkCommand : ICommand
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
                var network = new SimpleNeuralNetwork(RacerSample.SampleSize);

                var layerFactory = new NeuralLayerFactory();

                network.AddLayer(layerFactory.CreateNeuralLayer(200, new ReLUActivationFunction(), new WeightedSumFunction()));
                network.AddLayer(layerFactory.CreateNeuralLayer(200, new ReLUActivationFunction(), new WeightedSumFunction()));
                network.AddLayer(layerFactory.CreateNeuralLayer(200, new ReLUActivationFunction(), new WeightedSumFunction()));
                network.AddLayer(layerFactory.CreateNeuralLayer(100, new ReLUActivationFunction(), new WeightedSumFunction()));
                network.AddLayer(layerFactory.CreateNeuralLayer(4, new SigmoidActivationFunction(0.7), new WeightedSumFunction()));

                List<double[]> trainingSamples = new List<double[]>();
                List<double[]> expectedValues = new List<double[]>();

                string[] files = Directory.GetFiles("C:\\NeuralData\\0\\HAMILTON");

                List<Task> tasks = new List<Task>();
                ConcurrentQueue<string> readedSamples = new ConcurrentQueue<string>();

                foreach (string s in files)
                {
                    tasks.Add(Task.Factory.StartNew(() => readedSamples.Enqueue(File.ReadAllText(s))));
                }

                Task.WaitAll(tasks.ToArray());

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
                    inputs.Add(sample.WorldForwardDirX);
                    inputs.Add(sample.WorldForwardDirZ);
                    inputs.Add(sample.WorldRightDirX);
                    inputs.Add(sample.WorldRightDirZ);
                    inputs.Add(sample.Yaw);
                    inputs.Add(sample.Pitch);
                    inputs.Add(sample.Roll);

                    outputs.Add(sample.Throttle);
                    outputs.Add(sample.Steer);
                    outputs.Add(sample.CurrentGear / 9);

                    trainingSamples.Add(inputs.ToArray());
                    expectedValues.Add(outputs.ToArray());
                }

                network.PushExpectedValue(expectedValues.ToArray());
                network.Train(trainingSamples.ToArray(), 10000);
            }
        }
    }
}
