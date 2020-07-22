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

            for(int i = 0; i < hiddenLayers.Length; i++)
            {
                if(i == 0)
                {
                    _layers.Add(new NeuralLayer(hiddenLayers[i], "sigmoid", inputSize));
                    continue;
                }
                _layers.Add(new NeuralLayer(hiddenLayers[i], "sigmoid", hiddenLayers[i - 1]));
            }
            
            _layers.Add(new NeuralLayer(outputSize, "sigmoid", hiddenLayers.Last()));

            //Initialize(new Random(0));
        }

        public void Initialize(Random random)
        {
            foreach (NeuralLayer layer in _layers)
            {
                layer.Initialize(random);
            }
        }

        public float[] Process(float[] inputs)
        {
            // Console.WriteLine("Processing input...");
            for(int i = 0; i < _layers.Count; i++)
            {
                //Console.WriteLine("Firing layer {0}", _layers.IndexOf(layer));
                inputs = _layers[i].Fire(inputs);
            }
            return inputs;
        }
    }
}
