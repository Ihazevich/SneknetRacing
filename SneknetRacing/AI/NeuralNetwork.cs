using System;
using System.Collections.Generic;
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
        }

        private void Initialize(Random random)
        {
            Parallel.ForEach(_layers, (layer,status,index) =>
            {
                Console.WriteLine("Initializing layer {0}", index);
                layer.Initialize(random);
            }); 
        }
    }
}
