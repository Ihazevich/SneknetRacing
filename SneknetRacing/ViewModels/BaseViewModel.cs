using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private int _totalPackets;
        private BaseModel _packet;

        public int TotalPackets
        {
            get
            {
                return _totalPackets;
            }
            set
            {
                _totalPackets = value;
                OnPropertyChanged("TotalPackets");
            }
        }
        public BaseModel Packet
        {
            get
            {
                return _packet;
            }
            set
            {
                _packet = value;
                OnPropertyChanged("Packet");
            }
        }
        
        public BaseViewModel()
        {
            TotalPackets = 0;
        }

        public BaseViewModel(BaseModel packet)
        {
            TotalPackets = 0;
            Packet = packet;
        }

        public virtual void Desserialize(byte[] data)
        {
            TotalPackets++;
            Packet.Desserialize(data);
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
