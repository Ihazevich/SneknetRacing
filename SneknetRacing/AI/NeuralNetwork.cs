using MathNet.Numerics;
using MathNet.Numerics.Random;
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
        private Random _random = new Random();
        private List<NeuralLayer> _layers;

        public double Fitness { get; set; } = 9999999.0;
        public List<NeuralLayer> Layers { get { return _layers; } }

        public NeuralNetwork(int inputSize, int outputSize, int[] hiddenLayers)
        {
            if (hiddenLayers == null)
            {
                throw new ArgumentNullException(nameof(hiddenLayers));
            }

            _layers = new List<NeuralLayer>();

            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                if (i == 0)
                {
                    _layers.Add(new NeuralLayer(hiddenLayers[i], "relu", inputSize));
                    continue;
                }
                _layers.Add(new NeuralLayer(hiddenLayers[i], "relu", hiddenLayers[i - 1]));
            }

            _layers.Add(new NeuralLayer(outputSize, "sigmoid", hiddenLayers.Last()));
        }

        public NeuralNetwork(double[][][] networkWeights, bool[][] networkNodesStatus, int inputSize, double fitness)
        {
            if(networkWeights == null)
            {
                throw new ArgumentNullException(nameof(networkWeights));
            }

            if(networkNodesStatus == null)
            {
                throw new ArgumentNullException(nameof(networkNodesStatus));
            }

            _layers = new List<NeuralLayer>();
            for(int layerIndex = 0; layerIndex < networkWeights.Length; layerIndex++)
            {
                NeuralLayer layer;
                if (layerIndex == 0)
                {
                    layer = new NeuralLayer(networkWeights[layerIndex].Length, "relu", inputSize);
                }
                else if (layerIndex == networkWeights.Length - 1)
                {
                    layer = new NeuralLayer(networkWeights[layerIndex].Length, "sigmoid", networkWeights[layerIndex - 1].Length);
                }
                else
                {
                    layer = new NeuralLayer(networkWeights[layerIndex].Length, "relu", networkWeights[layerIndex - 1].Length);
                }
                layer.Load(networkWeights[layerIndex], networkNodesStatus[layerIndex]);
                _layers.Add(layer);
            }

            Fitness = fitness;
        }

        public void Initialize()
        {
            foreach (NeuralLayer layer in _layers)
            {
                layer.Initialize(_random);
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

        public void Test(double[][] samples, double[][] targets)
        {
            //Console.WriteLine("Network training started");

            Stopwatch stopwatch1 = Stopwatch.StartNew();
            Stopwatch stopwatch2 = new Stopwatch();

            double[][] output = new double[targets.Length][];
            double squaredErrorSum = 0;
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
                    squaredErrorSum += (err * err);
                    accuracy = (Math.Abs(output[i][j]) > Math.Abs(targets[i][j])) ? (Math.Abs(targets[i][j]) / Math.Abs(output[i][j])) : (Math.Abs(output[i][j] / targets[i][j]));
                }
                stopwatch2.Stop();
                //Console.WriteLine("Sample {0} processed in {1}ms. O: {2}, {3}, {4} | E: {5}, {6}, {7}",
                //i+1, stopwatch2.ElapsedMilliseconds, output[0], output[1], output[2], expectedValues[i][0], expectedValues[i][1], expectedValues[i][2]);
            }
            //squaredErrorSum /= (double)(samples.Length * samples[0].Length);
            accuracy /= (double)(samples.Length * samples[0].Length);

            stopwatch2.Stop();
            //Console.WriteLine("Processed {0} samples in {1}ms. Error: {2} | Accuracy: {3:##.####}%", samples.Length, stopwatch1.ElapsedMilliseconds, totalError, accuracy * 100);

            Fitness = squaredErrorSum;
        }

        public double[][][] GetWeights()
        {
            var networkWeights = new double[_layers.Count][][];

            for(int i = 0; i < _layers.Count; i++)
            {
                networkWeights[i] = _layers[i].GetWeights();
            }

            return networkWeights;
        }

        public bool[][] GetNodesStatus()
        {
            var nodesStatus = new bool[_layers.Count][];

            for (int i = 0; i < _layers.Count; i++)
            {
                nodesStatus[i] = _layers[i].GetNodesStatus();
            }

            return nodesStatus;
        }

        public void Mutate(double bestFitness)
        {
            double mutationSeverity = 1.0 - (1.0/Fitness);
            //Console.WriteLine("Mutation Severity: {0} | Fitness: {1} | Best: {2}", mutationSeverity, Fitness, bestFitness);

            mutationSeverity *= _random.NextDouble();

            double minNodes = 0.01;
            double maxNodes = 1;

            //Console.WriteLine("Mutation Severity after random: {0}", mutationSeverity);

            foreach (var layer in _layers)
            {
                var nodeMutationChance = minNodes + ((mutationSeverity * _random.NextDouble()) * (maxNodes - minNodes));
                var mutations = _random.NextDoubles(layer.Neurons.Count);
                int index = 0;
                foreach (var neuron in layer.Neurons)
                {
                    neuron.Mutate(mutationSeverity, _random);
                    neuron.isActive = (mutations[index] > nodeMutationChance);
                    index++;
                }
            }
        }
    }
}
