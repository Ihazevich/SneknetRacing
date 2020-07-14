using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private PacketHeader _header;
        private BaseModel _packet;

        public PacketHeader Header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
                OnPropertyChanged("Header");
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
            Header = new PacketHeader();
        }

        public BaseViewModel(BaseModel packet)
        {
            Header = new PacketHeader();
            Packet = packet;
        }

        public virtual void Desserialize(byte[] data)
        {
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
