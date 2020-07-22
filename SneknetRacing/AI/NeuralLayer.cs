using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.AI
{
    public class NeuralLayer
    {
        private List<Neuron> _neurons;

        public int Size
        {
            get 
            {
                return _neurons.Count;
            }
        }

        public List<Neuron> Neurons
        {
            get 
            {
                return _neurons;
            }
        }

        public NeuralLayer(int size)
        {
            _neurons = new List<Neuron>(size);
            for (int i = 0; i < size; i++)
            {
                _neurons.Add(new Neuron());
            }
        }

        public NeuralLayer(int size, string activation, int inputConnections)
        {
            _neurons = new List<Neuron>(size);
            for(int i = 0; i < size; i++)
            {
                _neurons.Add(new Neuron(activation, inputConnections));
            }
        }

        public float[] Fire(float[] inputs)
        {
            var output = new float[_neurons.Count];
            for(int i = 0; i < _neurons.Count; i++)
            {
                output[i] = _neurons[i].Fire(inputs);
            }

            return output;
        }

        public void Initialize(Random random)
        {
            foreach(var neuron in _neurons) 
            {
                neuron.SetWeights(random);
            }
            //Console.WriteLine("Layer initialization done");
        }
    }
}
