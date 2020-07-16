using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public HeaderViewModel()
        {
            Packet = new PacketHeader();
        }
    }
}
