using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class CarSetupsDataViewModel : BaseViewModel
    {
        public CarSetupsDataViewModel() : base(new PacketCarSetupData())
        {

        }
    }
}
