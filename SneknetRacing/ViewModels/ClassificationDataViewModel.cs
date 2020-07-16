using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class ClassificationDataViewModel : BaseViewModel
    {
        public ClassificationDataViewModel()
        {
            Packet = new PacketFinalClassificationData();
        }
    }
}
