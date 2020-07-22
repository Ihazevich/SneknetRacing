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

        public List<float> Output { get; private set; }

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

        public void Fire()
        {
            foreach(var neuron in _neurons)
            {
                neuron.Fire();
            }
            Output = _neurons.Select(neuron => neuron.Output).ToList();
            foreach (var output in Output)
            {
                Console.Write("{0}, ", output);
            }
        }

        public void PushInputs(List<float> inputs)
        {
            for(int i = 0; i < inputs.Count; i++) 
            {
                _neurons[i].PushInputValue(inputs[i]);
            }
        }

        public void Initialize(Random random)
        {
            foreach(var neuron in _neurons) 
            {
                neuron.SetWeights(random);
            }
            Console.WriteLine("Layer initialization done");
        }

        public void SetAsInputLayer()
        {
            foreach(var neuron in _neurons)
            {
                neuron.SetAsInputNeuron();
            }
        }

        public void Connect(List<Neuron> previousLayerNeurons)
        {
            Parallel.ForEach(_neurons, new ParallelOptions { MaxDegreeOfParallelism = 2 }, neuron =>
            {
                neuron.Connect(previousLayerNeurons);
            });
        }
    }
}
