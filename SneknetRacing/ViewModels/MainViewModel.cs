using SneknetRacing.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SneknetRacing.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            { 
                _selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }

        public ICommand UpdateViewCommand { get; set; } 

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
        }



    }
}
