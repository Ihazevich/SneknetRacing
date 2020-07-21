using SneknetRacing.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.AI
{
    public class NeuralNetwork
    {
        private List<NeuralLayer> _layers;

        public NeuralNetwork()
        {
            _layers = new List<NeuralLayer>();
        }

        public NeuralNetwork(int inputSize, int outputSize, int[] hiddenLayers) : this()
        {
            _layers.Add(new NeuralLayer(inputSize));
            for(int i = 0; i < hiddenLayers.Length; i++)
            {
                if(i == 0)
                {
                    _layers.Add(new NeuralLayer(hiddenLayers[i], "relu", inputSize));
                    continue;
                }
                _layers.Add(new NeuralLayer(hiddenLayers[i], "relu", hiddenLayers[i - 1]));
            }
            _layers.Add(new NeuralLayer(outputSize, "tanh", hiddenLayers.Last()));

            _layers.First().SetAsInputLayer();
            Initialize(new Random(0));
            Connect();
        }

        private void Initialize(Random random)
        {
            foreach (NeuralLayer layer in _layers)
            {
                // Skip the input layer
                if (_layers.IndexOf(layer) == 0)
                {
                    continue;
                }
                layer.Initialize(random);
            }
        }

        private void Connect()
        {
            foreach(NeuralLayer layer in _layers)
            {
                // Skip the input layer
                if(_layers.IndexOf(layer) == 0)
                {
                    continue;
                }
                layer.Connect(_layers[_layers.IndexOf(layer)-1].Neurons);
            }
        }

        public List<float> Process(List<float> inputs)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Processing input...");
            var outputs = new List<float>(_layers.Last().Size);

            _layers.First().PushInputs(inputs);

            foreach(NeuralLayer layer in _layers)
            {
                Console.WriteLine("Firing layer {0}", _layers.IndexOf(layer));
                layer.Fire();
            }
            stopwatch.Stop();
            Console.WriteLine("Done in {0}ms", stopwatch.ElapsedMilliseconds);
            return _layers.Last().Output;
        }
    }
}
