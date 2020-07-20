using SneknetRacing.AI.Neurons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SneknetRacing.AI
{
    public class NeuralLayer
    {
        public List<INeuron> Neurons;

        public NeuralLayer()
        {
            Neurons = new List<INeuron>();
        }

        public void ConnectLayer(NeuralLayer inputLayer)
        {
            var combos = Neurons.SelectMany(neuron => inputLayer.Neurons, (neuron, input) => new { neuron, input });
            combos.ToList().ForEach(x => x.neuron.AddInputNeuron(x.input));
        }
    }
}
