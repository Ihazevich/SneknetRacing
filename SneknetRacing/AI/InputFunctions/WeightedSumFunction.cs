using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneknetRacing.AI.InputFunctions
{
    public class WeightedSumFunction : IInputFunction
    {
        public double CalculateInput(List<ISynapse> inputs)
        {
            return inputs.Select(x => x.Weight * x.GetOutput()).Sum();

            /*
            var tasks = new List<Task>();
            var outputs = new List<double>();

            inputs.ForEach(x =>
            {
                tasks.Add(Task.Factory.StartNew(() => x.GetOutput() * x.Weight));
            }).asParallel();

            Task.WaitAll(tasks.ToArray());

            return outputs.Sum();
            */
        }
    }
}
