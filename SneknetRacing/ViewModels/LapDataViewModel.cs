using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class LapDataViewModel : BaseViewModel
    {
        public LapDataViewModel()
        {
            Packet = new PacketLapData();
        }
    }
}
