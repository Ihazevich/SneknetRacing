using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.AI
{
    public interface IActivationFunction
    {
        double CalculateOutput(double input);
    }
}
