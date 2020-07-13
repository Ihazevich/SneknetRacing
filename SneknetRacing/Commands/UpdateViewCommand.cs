using SneknetRacing.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SneknetRacing.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel _viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter.ToString() == "Header")
            {
                _viewModel.SelectedViewModel = new HeaderViewModel();
            }
        }
    }
}
