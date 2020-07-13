using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SneknetRacing.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
