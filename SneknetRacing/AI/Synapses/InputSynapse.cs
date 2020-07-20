using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.AI.Synapses
{
    public class InputSynapse : ISynapse
    {
        internal INeuron _toNeuron;

        public double Weight { get; set; }
        public double Output { get; set; }
        public double PreviousWeight { get; set; }

        public InputSynapse(INeuron toNeuron)
        {
            _toNeuron = toNeuron;
            Weight = 1;
        }

        public InputSynapse(INeuron toNeuron, double output)
        {
            _toNeuron = toNeuron;
            Output = output;
            Weight = 1;
            PreviousWeight = 1;
        }

        public double GetOutput()
        {
            return Output;
        }

        public bool IsFromNeuron(Guid fromNeuroId)
        {
            return false;
        }

        public void UpdateWeight(double learningRate, double delta)
        {
            throw new InvalidOperationException("Input Connection cant have its weight changed");
        }
    }
}
