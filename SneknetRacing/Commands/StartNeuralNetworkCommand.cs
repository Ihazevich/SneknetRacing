using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.ML.Calibrators;
using MxNet;
using MxNet.Gluon;
using MxNet.Gluon.Losses;
using MxNet.Gluon.NN;
using MxNet.Initializers;
using MxNet.IO;
using MxNet.Metrics;
using MxNet.Optimizers;
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

                var batchSize = 1000;
                var pog = new NDArray(trainingSamples.ToArray());
                var trainData = new NDArrayIter(pog[0],null,batchSize,true);

                // Define simple network with dense layers
                var net = new Sequential();
                net.Add(new Dense(128, ActivationType.Relu));
                net.Add(new Dense(64, ActivationType.Relu));
                net.Add(new Dense(10));

                //Set context, multi-gpu supported
                var gpus = TestUtils.ListGpus();
                //var ctx = gpus.Count > 0 ? gpus.Select(x => Context.Gpu(x)).ToArray() : new[] { Context.Cpu(0) };
                var ctx = new[] { Context.Cpu(0) };

                //Initialize the weights
                net.Initialize(new Xavier(magnitude: 2.24f), ctx);

                //Create the trainer with all the network parameters and set the optimizer
                var trainer = new Trainer(net.CollectParams(), new Adam());

                var epoch = 10;
                var metric = new Accuracy(); //Use Accuracy as the evaluation metric.
                var softmax_cross_entropy_loss = new SoftmaxCELoss();
                float lossVal = 0; //For loss calculation
                for (var iter = 0; iter < epoch; iter++)
                {
                    var tic = DateTime.Now;
                    // Reset the train data iterator.
                    trainData.Reset();
                    lossVal = 0;

                    // Loop over the train data iterator.
                    while (!trainData.End())
                    {
                        var batch = trainData.Next();

                        // Splits train data into multiple slices along batch_axis
                        // and copy each slice into a context.
                        var data = Utils.SplitAndLoad(batch.Data[0], ctx, batch_axis: 0);

                        // Splits train labels into multiple slices along batch_axis
                        // and copy each slice into a context.
                        var label = Utils.SplitAndLoad(batch.Label[0], ctx, batch_axis: 0);

                        var outputs = new NDArrayList();

                        // Inside training scope
                        using (var ag = Autograd.Record())
                        {
                            outputs = Enumerable.Zip(data, label, (x, y) =>
                            {
                                var z = net.Call(x);

                                // Computes softmax cross entropy loss.
                                NDArray loss = softmax_cross_entropy_loss.Call(z, y);

                                // Backpropagate the error for one iteration.
                                loss.Backward();
                                lossVal += loss.Mean();
                                return z;
                            }).ToList();
                        }

                        // Updates internal evaluation
                        metric.Update(label, outputs.ToArray());

                        // Make one step of parameter update. Trainer needs to know the
                        // batch size of data to normalize the gradient by 1/batch_size.
                        trainer.Step(batch.Data[0].Shape[0]);
                    }

                    var toc = DateTime.Now;

                    // Gets the evaluation result.
                    var (name, acc) = metric.Get();

                    // Reset evaluation result to initial state.
                    metric.Reset();
                    Console.Write($"Loss: {lossVal} ");
                    Console.WriteLine($"Training acc at epoch {iter}: {name}={(acc * 100).ToString("0.##")}%, Duration: {(toc - tic).TotalSeconds.ToString("0.#")}s");
                }
            }
        }
    }
}
