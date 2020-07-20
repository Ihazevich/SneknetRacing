using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.AI
{
    public class NeuralLayerFactory
    {
        public NeuralLayer CreateNeuralLayer(int numberOfNeurons, IActivationFunction activationFucntion, IInputFunction inputFunction)
        {
            var layer = new NeuralLayer();

            for(int i = 0; i < numberOfNeurons; i++)
            {
                var neuron = new Neuron(activationFucntion, inputFunction);
                layer.Neurons.Add(neuron);
            }

            return layer;
        }
    }
}
