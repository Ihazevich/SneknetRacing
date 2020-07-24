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
        private string _activation;

        public List<double> Weights
        {
            get
            {
                return _weights.ToList();
            }
        }

        public bool isActive { get; set; }  

        public Neuron()
        {
            _weights = Array.Empty<double>();
            _activation = "";
        }

        public Neuron(string activation, int inputConnections) : this()
        {
            _weights = new double[inputConnections];
            isActive = true;
            _activation = activation;
        }

        public double Fire(double[] inputs)
        {
            if(inputs == null)
            {
                throw new ArgumentNullException(nameof(inputs));
            }

            var output = 0.0;
            if (isActive)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    output += (inputs[i] * _weights[i]);
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
                double u1 = 1.0 - random.NextDouble(); //uniform(0,1] random doubles
                double u2 = 1.0 - random.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

                _weights[i] = randStdNormal;
            }
        }

        public void Mutate(double mutationSeverity, Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            var mutations = random.NextDoubles(_weights.Length);
            var speed = 0.0000001;

            for (int i = 0; i < _weights.Length; i++)
            {
                var change = ((mutations[i] * (mutationSeverity * 2.0)) - mutationSeverity) * speed;
                //Console.WriteLine("W: {0} | Mutation: {1}", _weights[i], change);
                _weights[i] -= change;
                //Console.WriteLine("W: {0}", _weights[i]);
            }
        }

        public void LoadWeights(double[] weights, bool nodeActive)
        {
            _weights = weights;
            isActive = nodeActive;
        }
    }
}
