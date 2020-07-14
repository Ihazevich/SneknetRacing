using SneknetRacing.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SneknetRacing.Commands
{
    public class StartServerCommand : ICommand
    {
        private MainViewModel _viewModel;

        public StartServerCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !_viewModel.NetworkThreadsRunning;
        }

        public void Execute(object parameter)
        {
            if (_viewModel.NetworkThreadsRunning)
            {
            }
            else
            {
                // Start Server thread
                _viewModel.ServerThread.Start();
                // Start Handler thread
                _viewModel.DataHandlerThread.Start();
                _viewModel.NetworkThreadsRunning = true;
            }

        }
    }
}
