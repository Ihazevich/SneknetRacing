using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SneknetRacing.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private long _totalPackets = 0;

        private Stopwatch _stopwatch;

        private MainViewModel _parentView;

        public long TotalPackets
        {
            get
            {
                return _totalPackets;
            }
            set
            {
                _totalPackets = value;
                OnPropertyChanged("TotalPackets");
                OnPropertyChanged("PacketsPerSecond");
            }
        }

        public string PacketsPerSecond
        {
            get 
            {
                return "Packets: " + TotalPackets + " | Ellapsed time in seconds: " + _stopwatch.ElapsedMilliseconds / 1000 + " | Rate: " + TotalPackets / (_stopwatch.ElapsedMilliseconds / 1000) + " packets/sec";
            }
        }

        public Task DesserializationThread { get; set; }
        
        public BaseViewModel()
        {
            _stopwatch = Stopwatch.StartNew(); 
        }

        public BaseViewModel(MainViewModel parentView) : this()
        {
            _parentView = parentView;
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
