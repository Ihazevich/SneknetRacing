using MathNet.Numerics;
using MathNet.Numerics.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.AI
{
    public class Neuron
    {
        private double[] _weights;
        private bool[] _nodesStatus;
        private string _activation;

        public double[] Weights
        {
            get
            {
                return _weights;
            }
        }

        public Neuron()
        {
            _weights = Array.Empty<double>();
            _activation = "";
        }

        public Neuron(string activation, int inputConnections) : this()
        {
            _weights = new double[inputConnections];
            _nodesStatus = new bool[inputConnections];
            _activation = activation;
        }

        public double Fire(double[] inputs)
        {
            if(inputs == null)
            {
                throw new ArgumentNullException(nameof(inputs));
            }

            var output = 0.0;

            for (int i = 0; i < inputs.Length; i++)
            {
                if(_nodesStatus[i])
                {
                    output += (inputs[i] * _weights[i]);
                }
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
            if(random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            //Console.WriteLine("Setting weights for {0} connections", _inputWeights.Capacity);
            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] = (random.NextDouble() * 2.0) - 1.0;
                _nodesStatus[i] = true;
            }
        }

        public void Mutate(double mutationSeverity, Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            var mutations = random.NextDoubles(_weights.Length);

            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] -= (mutations[i] * (mutationSeverity * 2.0)) - mutationSeverity;
            }

            double minNodes = 0.2;
            double maxNodes = 1;

            var nodeMutation = minNodes + ((mutationSeverity * random.NextDouble()) * (maxNodes - minNodes));

            mutations = random.NextDoubles(_weights.Length);

            for (int i = 0; i < _nodesStatus.Length; i++)
            {
                _nodesStatus[i] = (mutations[i] > nodeMutation);
            }
        }

        public void LoadWeights(double[] weights)
        {
            _weights = weights;
        }
    }
}
