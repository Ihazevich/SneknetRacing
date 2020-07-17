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
using System.Windows.Threading;
using System.Windows;

namespace SneknetRacing.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private long _totalPackets = 0;
        private Stopwatch _stopwatch;

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

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(new Action(() => PropertyChanged(this, new PropertyChangedEventArgs(propertyName))));
                }
            }
        }
        #endregion
    }
}

