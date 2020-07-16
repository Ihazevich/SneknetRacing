using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class CarTelemetryDataViewModel : BaseViewModel
    {
        public CarTelemetryDataViewModel()
        {
            Packet = new PacketCarTelemetryData();
        }
    }
}
