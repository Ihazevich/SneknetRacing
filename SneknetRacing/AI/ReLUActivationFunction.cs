using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.AI
{
    public class ReLUActivationFunction : IActivationFunction
    {
        public double CalculateOutput(double input)
        {
            return Math.Max(0, input);
        }
    }
}
