using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class EventDataViewModel :  BaseViewModel
    {
        public EventDataViewModel()
        {
            Packet = new PacketEventData();
        }
    }
}
