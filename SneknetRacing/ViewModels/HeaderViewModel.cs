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
        public BindingList<PacketHeader> PacketHeaders { get; set; }

        public HeaderViewModel() : base(new PacketHeader())
        {

        }

    }
}
