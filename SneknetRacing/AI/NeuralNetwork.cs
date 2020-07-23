using SneknetRacing.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.AI
{
    public class NeuralNetwork
    {
        private List<NeuralLayer> _layers;

        public NeuralNetwork(List<List<List<double>>> networkWeights, int inputSize)
        {
            _layers = new List<NeuralLayer>();
            for(int layerIndex = 0; layerIndex < networkWeights.Count; layerIndex++)
            {
                NeuralLayer layer;
                if(layerIndex == 0)
                {
                    layer = new NeuralLayer(networkWeights[layerIndex].Count, "sigmoid", inputSize);
                }
                else
                {
                    layer = new NeuralLayer(networkWeights[layerIndex].Count, "sigmoid", networkWeights[layerIndex - 1].Count);
                }
                layer.Load(networkWeights[layerIndex]);
                _layers.Add(layer);
            }
        }

        public NeuralNetwork(int inputSize, int outputSize, int[] hiddenLayers)
        {
            _layers = new List<NeuralLayer>();
            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                if (i == 0)
                {
                    _layers.Add(new NeuralLayer(hiddenLayers[i], "sigmoid", inputSize));
                    continue;
                }
                _layers.Add(new NeuralLayer(hiddenLayers[i], "sigmoid", hiddenLayers[i - 1]));
            }

            _layers.Add(new NeuralLayer(outputSize, "sigmoid", hiddenLayers.Last()));
        }

        public void Initialize(Random random)
        {
            foreach (NeuralLayer layer in _layers)
            {
                layer.Initialize(random);
            }
        }

        public double[] Process(double[] inputs)
        {
            // Console.WriteLine("Processing input...");
            for (int i = 0; i < _layers.Count; i++)
            {
                //Console.WriteLine("Firing layer {0}", _layers.IndexOf(layer));
                inputs = _layers[i].Fire(inputs);
            }
            return inputs;
        }

        public double Test(double[][] samples, double[][] targets)
        {
            //Console.WriteLine("Network training started");

            Stopwatch stopwatch1 = Stopwatch.StartNew();
            Stopwatch stopwatch2 = new Stopwatch();

            double[][] output = new double[targets.Length][];
            double totalError = 0;
            double accuracy = 0;

            for (int i = 0; i < samples.Length; i++)
            {
                /*Console.Write("Inputs:");
                foreach(var input in trainingSamples[i])
                {
                    Console.Write("{0}, ", input);
                }
                Console.WriteLine();*/
                stopwatch2.Restart();
                output[i] = Process(samples[i]);
                for (int j = 0; j < output[0].Length; j++)
                {
                    double err = output[i][j] - targets[i][j];
                    totalError += (err * err);
                    accuracy = (output[i][j] > targets[i][j]) ? (targets[i][j] / output[i][j]) : (output[i][j] / targets[i][j]);
                }
                stopwatch2.Stop();
                //Console.WriteLine("Sample {0} processed in {1}ms. O: {2}, {3}, {4} | E: {5}, {6}, {7}",
                //i+1, stopwatch2.ElapsedMilliseconds, output[0], output[1], output[2], expectedValues[i][0], expectedValues[i][1], expectedValues[i][2]);
            }
            totalError /= (double)(samples.Length * samples[0].Length);
            accuracy /= (double)(samples.Length * samples[0].Length);

            stopwatch2.Stop();
            //Console.WriteLine("Processed {0} samples in {1}ms. Error: {2} | Accuracy: {3:##.####}%", samples.Length, stopwatch1.ElapsedMilliseconds, totalError, accuracy * 100);

            return ( 1 - totalError) * accuracy;
        }

        public List<List<List<double>>> GetWeights()
        {
            var networkWeights = new List<List<List<double>>>();

            foreach(var layer in _layers)
            {
                var layerWeights = new List<List<double>>();

                foreach (var neuron in layer.Neurons)
                {
                    var neuronWeights = new List<double>();

                    foreach(var weight in neuron.Weights)
                    {
                        neuronWeights.Add(weight);
                    }

                    layerWeights.Add(neuronWeights);
                }

                networkWeights.Add(layerWeights);
            }

            return networkWeights;
        }

        public void Mutate(double mutationChance)
        {
            Random rand = new Random();
            List<Task> temp = new List<Task>();

            foreach(var layer in _layers)
            {
                foreach(var neuron in layer.Neurons)
                {
                    temp.Add(new Task(() => neuron.Mutate(mutationChance)));
                }
            }

            Task[] tasks = temp.ToArray();

            foreach(var task in tasks)
            {
                task.Start();
            }

            Task.WaitAll(tasks);
        }
    }
}
