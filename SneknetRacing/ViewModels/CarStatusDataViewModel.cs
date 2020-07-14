using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class CarStatusDataViewModel : BaseViewModel
    {
        public CarStatusDataViewModel() : base(new PacketCarStatusData())
        {

        }
    }
}
