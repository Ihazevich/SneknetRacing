using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.AI
{
    public class Neuron
    {
        private List<Neuron> _inputConnections;
        private List<float> _inputWeights;
        private string _activation;

        public float Output { get; private set; }

        public Neuron()
        {
            _inputConnections = new List<Neuron>(0);
            _inputWeights = new List<float>(0);
            _activation = "";
            Output = 0.0f;
        }

        public Neuron(string activation, int inputConnections) : this()
        {
            _inputConnections = new List<Neuron>(inputConnections);
            _inputWeights = new List<float>(inputConnections);
            _activation = activation;
        }

        public void Connect(List<Neuron> inputs)
        {
            _inputConnections = inputs;
        }

        public void Fire()
        {
            if(_activation == "input")
            {
                //Console.WriteLine(Output);
                return;
            }

            Parallel.ForEach(_inputConnections, neuron =>
            {
                Output += neuron.Output * _inputWeights[_inputConnections.IndexOf(neuron)];
            });

            switch (_activation)
            {
                case "relu":
                    if(Output < 0)
                    {
                        Output *= 0.01f;
                    }
                    break;
                case "tanh":
                    Output = (float)Math.Tanh(Output);
                    break;
            }

            //Console.WriteLine(Output);
        }

        public void SetWeights(Random random)
        {
            if(_activation == "input")
            {
                return;
            }
            //Console.WriteLine("Setting weights for {0} connections", _inputWeights.Capacity);
            Parallel.For(0, _inputWeights.Capacity, index =>
            {
                _inputWeights.Add((float)random.NextDouble() - (float)random.NextDouble());
            });
        }

        public void SetAsInputNeuron()
        {
            _activation = "input";
        }

        public void PushInputValue(float value)
        {
            if(_activation == "input")
            {
                Output = value;
            }
            else
            {
                throw new Exception("Trying to push an input value to a non-input neuron");
            }
        }
    }
}
