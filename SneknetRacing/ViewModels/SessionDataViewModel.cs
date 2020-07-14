using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class SessionDataViewModel : BaseViewModel
    {
        public SessionDataViewModel() : base(new PacketSessionData())
        {

        }
    }
}
