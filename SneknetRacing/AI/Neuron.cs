using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.AI
{
    public class Neuron
    {
        private double[] _inputWeights;
        private string _activation;

        public List<double> Weights
        {
            get
            {
                return _inputWeights.ToList();
            }
        }

        public Neuron()
        {
            _inputWeights = new double[0];
            _activation = "";
        }

        public Neuron(string activation, int inputConnections) : this()
        {
            _inputWeights = new double[inputConnections];
            _activation = activation;
        }

        public void Connect(List<Neuron> inputs)
        {
            //_inputConnections = inputs;
        }

        public double Fire(double[] inputs)
        {
            var output = 0.0;

            for (int i = 0; i < inputs.Length; i++)
            {
                output += (inputs[i] * _inputWeights[i]);
            }

            switch (_activation)
            {
                case "relu":
                    if (output < 0)
                    {
                        output *= 0.01;
                    }
                    break;
                case "tanh":
                    output = Math.Tanh(output);
                    break;
                case "sigmoid":
                    output = 1.0 / (1.0 + Math.Pow(Math.E, -output));
                    output *= 2;
                    output--;
                    break;
            }

            return output;

            //Console.WriteLine(Output);
        }

        public void SetWeights(Random random)
        {
            //Console.WriteLine("Setting weights for {0} connections", _inputWeights.Capacity);
            for (int i = 0; i < _inputWeights.Length; i++)
            {
                _inputWeights[i] = random.NextDouble() - random.NextDouble();
            }
        }

        public void Mutate(double mutationChance)
        {
            Random rand = new Random();
            for (int i = 0; i < _inputWeights.Length; i++)
            {
                if(mutationChance > rand.NextDouble())
                {
                    _inputWeights[i] = rand.NextDouble() - rand.NextDouble();
                }
            }
        }

        public void LoadWeights(double[] weights)
        {
            _inputWeights = weights;
        }
    }
}
