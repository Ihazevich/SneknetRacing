using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace SneknetRacing.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public abstract BaseModel Desserialize(byte[] data);

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
