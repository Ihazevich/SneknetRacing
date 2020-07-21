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

        public List<float> Output
        {
            get
            {
                return _neurons.Select(neuron => neuron.Output).ToList();
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
            Parallel.For(0, size, x => {
                _neurons.Add(new Neuron());
            });
        }

        public NeuralLayer(int size, string activation, int inputConnections)
        {
            _neurons = new List<Neuron>();
            Parallel.For(0, size, x =>{
                _neurons.Add(new Neuron(activation, inputConnections));
            });
        }

        public void Fire()
        {
            Parallel.ForEach(_neurons, neuron =>
            {
                neuron.Fire();
            });
        }

        public void PushInputs(List<float> inputs)
        {
            Parallel.ForEach(_neurons, (neuron, state, index) =>
            {
                neuron.PushInputValue(inputs[(int)index]);
            });
        }

        public void Initialize(Random random)
        {
            Parallel.ForEach(_neurons, (neuron, state, index) =>
            {
                Console.Write(index);
                neuron.SetWeights(random);
            });
            Console.WriteLine("Layer initialization done");
        }

        public void SetAsInputLayer()
        {
            Parallel.ForEach(_neurons, neuron =>
            {
                neuron.SetAsInputNeuron();
            });
        }

        public void Connect(List<Neuron> previousLayerNeurons)
        {
            Parallel.ForEach(_neurons, neuron =>
            {
                neuron.Connect(previousLayerNeurons);
            });
        }
    }
}
