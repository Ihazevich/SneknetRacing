using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.AI
{
    public class SimpleNeuralNetwork
    {
        private NeuralLayerFactory _layerFactory;
        private Stopwatch stopwatch = new Stopwatch();

        internal List<NeuralLayer> _layers;
        internal double _learningRate;
        internal double[][] _expectedResult;

        public SimpleNeuralNetwork(int numberOfInputNeurons)
        {
            _layers = new List<NeuralLayer>();
            _layerFactory = new NeuralLayerFactory();

            CreateInputLayer(numberOfInputNeurons);

            _learningRate = 1.5;
        }

        public void AddLayer(NeuralLayer newLayer)
        {
            if(_layers.Any())
            {
                var lastLayer = _layers.Last();
                newLayer.ConnectLayer(lastLayer);
            }

            _layers.Add(newLayer);
        }

        public void PushInputValues(double[] inputs)
        {
            _layers.First().Neurons.ForEach(x => x.PushValueOnInput(inputs[_layers.First().Neurons.IndexOf(x)]));
        }

        public void PushExpectedValue(double[][] expectedOutputs)
        {
            _expectedResult = expectedOutputs;
        }

        public List<double> GetOutput()
        {
            var returnValue = new List<double>();

            _layers.Last().Neurons.ForEach(neuron =>
            {
                returnValue.Add(neuron.CalculateOutput());
            });

            return returnValue;
        }

        public void Train(double[][] inputs, int numberOfEpochs)
        {
            double totalError = 0;
            for(int i = 0; i < numberOfEpochs; i++)
            {
                Console.WriteLine("Eppoch {0}", i);
                stopwatch.Restart();
                for(int j = 0; j < inputs.GetLength(0); j++)
                {
                    PushInputValues(inputs[j]);

                    var outputs = new List<double>();
                    var tasks = new List<Task>();

                    /*Parallel.ForEach(_layers.Last().Neurons, x =>
                    {
                        outputs.Add(x.CalculateOutput());
                    });
                    */
                    /*_layers.Last().Neurons.ForEach(x =>
                    {
                        tasks.Add(Task.Factory.StartNew(() => outputs.Add(x.CalculateOutput())));
                    });*/
                    
                    _layers.Last().Neurons.ForEach(x => outputs.Add(x.CalculateOutput()));
                    //Task.WaitAll(tasks.ToArray());
                    totalError = CalculateTotalError(outputs, j);
                    stopwatch.Stop();
                    Console.WriteLine("Sample {0} processed in {1}ms | Outputs: {2},{3},{4} | Expected: {5},{6},{7} | Error: {8}", j, stopwatch.ElapsedMilliseconds, outputs[0], outputs[1], outputs[2], _expectedResult[j][0], _expectedResult[j][1], _expectedResult[j][2], totalError);
                    HandleOutputLayer(j);
                    HandleHiddenLayers();
                }
                Console.WriteLine("Eppoch: {0} | Total Error: {1}", i, totalError);            }
        }

        public void CreateInputLayer(int numberOfInputNeurons)
        {
            var inputLayer = _layerFactory.CreateNeuralLayer(numberOfInputNeurons, new ReLUActivationFunction(), new WeightedSumFunction());
            inputLayer.Neurons.ForEach(x => x.AddInputSynapse(0));
            this.AddLayer(inputLayer);
        }

        private double CalculateTotalError(List<double> outputs, int row)
        {
            double totalError = 0;

            outputs.ForEach(output =>
            {
                var error = Math.Pow(output - _expectedResult[row][outputs.IndexOf(output)], 2);
                totalError += error;
            });

            return totalError;
        }

        private void HandleOutputLayer(int row)
        {
            _layers.Last().Neurons.ForEach(neuron =>
            {
                neuron.Inputs.ForEach(connection =>
                {
                    var output = neuron.CalculateOutput();
                    var netInput = connection.GetOutput();

                    var expectedOutput = _expectedResult[row][_layers.Last().Neurons.IndexOf(neuron)];

                    var nodeDelta = (expectedOutput - output) * output * (1 - output);
                    var delta = -1 * netInput * nodeDelta;

                    connection.UpdateWeight(_learningRate, delta);

                    neuron.PreviousPartialDerivate = nodeDelta;
                });
            });
        }

        private void HandleHiddenLayers()
        {
            for(int k = _layers.Count - 2; k > 0; k--)
            {
                _layers[k].Neurons.ForEach(neuron =>
                {
                    neuron.Inputs.ForEach(connection =>
                    {
                        var output = neuron.CalculateOutput();
                        var netInput = connection.GetOutput();
                        double sumPartial = 0;

                        _layers[k + 1].Neurons.ForEach(outputNeuron =>
                        {
                            outputNeuron.Inputs.Where(i => i.IsFromNeuron(neuron.Id)).ToList().ForEach(outConnection =>
                            {
                                sumPartial += outConnection.PreviousWeight * outputNeuron.PreviousPartialDerivate;
                            });
                        });

                        var delta = -1 * netInput * sumPartial * output * (1 - output);
                        connection.UpdateWeight(_learningRate, delta);
                    });
                });
            }
        }
    }
}
