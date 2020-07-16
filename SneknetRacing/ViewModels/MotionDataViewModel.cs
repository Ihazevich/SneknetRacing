using SneknetRacing.Models;
using SneknetRacing.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class MotionDataViewModel : BaseViewModel
    {
        private CarMotionData _selectedCarMotionData = new CarMotionData();

        public SelectedCarMotionDataView SelectedCarMotionDataView { get; }

        public CarMotionData SelectedCarMotionData
        {
            get
            {
                return _selectedCarMotionData;
            }
            set
            {
                _selectedCarMotionData = value;
                OnPropertyChanged("SelectedCarMotionData");
            }
        }

        public MotionDataViewModel()
        {
            Packet = new PacketMotionData();
            SelectedCarMotionDataView = new SelectedCarMotionDataView();
        }
    }
}
