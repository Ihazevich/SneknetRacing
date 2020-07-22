using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.AI
{
    public class Neuron
    {
        private float[] _inputWeights;
        private string _activation;

        public Neuron()
        {
            _inputWeights = new float[0];
            _activation = "";
        }

        public Neuron(string activation, int inputConnections) : this()
        {
            _inputWeights = new float[inputConnections];
            _activation = activation;
        }

        public void Connect(List<Neuron> inputs)
        {
            //_inputConnections = inputs;
        }

        public float Fire(float[] inputs)
        {
            var output = 0.0f;

            for(int i = 0; i < inputs.Length; i++)
            {
                output += (inputs[i] * _inputWeights[i]);
            }

            switch (_activation)
            {
                case "relu":
                    if(output < 0)
                    {
                        output *= 0.01f;
                    }
                    break;
                case "tanh":
                    output = (float)Math.Tanh(output);
                    break;
                case "sigmoid":
                    output =  1.0f / (1.0f + (float)Math.Pow(Math.E, -output));
                    break;
            }

            return output;

            //Console.WriteLine(Output);
        }

        public void SetWeights(Random random)
        {
            if(_activation == "input")
            {
                return;
            }
            //Console.WriteLine("Setting weights for {0} connections", _inputWeights.Capacity);
            for(int i = 0; i < _inputWeights.Length; i++)
            {
                _inputWeights[i] = (float)random.NextDouble() - (float)random.NextDouble();
            }
        }

        public void SetAsInputNeuron()
        {
            _activation = "input";
        }
    }
}
