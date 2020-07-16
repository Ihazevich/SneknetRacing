using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class MotionDataViewModel : BaseViewModel
    {
        public MotionDataViewModel() : base(new PacketMotionData())
        {
            
        }
    }
}
