﻿using SneknetRacing.AI.ActivationFunctions;
using SneknetRacing.AI.InputFunctions;
using SneknetRacing.AI.Synapses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SneknetRacing.AI.Neurons
{
    public class Neuron : INeuron
    {
        private IActivationFunction _activationFunction;
        private IInputFunction _inputFunction;

        public List<ISynapse> Inputs { get; set; }

        public List<ISynapse> Outputs { get; set; }

        public Guid Id { get; private set; }

        public double PreviousPartialDerivate { get; set; }

        public Neuron(IActivationFunction activationFunction, IInputFunction inputFunction)
        {
            Id = Guid.NewGuid();
            Inputs = new List<ISynapse>();
            Outputs = new List<ISynapse>();

            _activationFunction = activationFunction;
            _inputFunction = inputFunction;
        }

        public void AddInputNeuron(INeuron inputNeuron)
        {
            var synapse = new Synapse(inputNeuron, this);
            Inputs.Add(synapse);
            inputNeuron.Outputs.Add(synapse);
        }

        public void AddOutputNeuron(INeuron outputNeuron)
        {
            var synapse = new Synapse(this, outputNeuron);
            Outputs.Add(synapse);
            outputNeuron.Inputs.Add(synapse);
        }

        public double CalculateOutput()
        {
            var output = _activationFunction.CalculateOutput(_inputFunction.CalculateInput(this.Inputs));
            return output;
        }

        public void AddInputSynapse(double inputValue)
        {
            var inputSynapse = new InputSynapse(this, inputValue);
            Inputs.Add(inputSynapse);
        }

        public void PushValueOnInput(double inputValue)
        {
            ((InputSynapse)Inputs.First()).Output = inputValue;
        }
    }
}