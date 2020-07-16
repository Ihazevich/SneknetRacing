using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class ParticipantsDataViewModel : BaseViewModel
    {
        public ParticipantsDataViewModel()
        {
            Packet = new PacketParticipantsData();
        }
    }
}
