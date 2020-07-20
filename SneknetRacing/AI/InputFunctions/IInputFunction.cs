using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.AI.InputFunctions
{
    public interface IInputFunction
    {
        double CalculateInput(List<ISynapse> inputs);
    }
}
