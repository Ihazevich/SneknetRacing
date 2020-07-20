using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.AI.ActivationFunctions
{
    public interface IActivationFunction
    {
        double CalculateOutput(double input);
    }
}
