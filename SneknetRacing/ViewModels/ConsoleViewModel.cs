using SneknetRacing.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class ConsoleViewModel : BaseViewModel
    {
        public Logger Logger { get; set; }
        public ConsoleViewModel(Logger logger)
        {
            Logger = logger;
            Logger.Log("poggies");
        }
    }
}
