using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MathNet.Numerics;
using SneknetRacing.AI;
using SneknetRacing.ViewModels;


namespace SneknetRacing.Commands
{
    public class StartNeuralNetworkCommand : ICommand
    {
        private bool _isNetworkRunning = false;
        private MainViewModel _viewModel;

        public StartNeuralNetworkCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !_isNetworkRunning;
        }

        public void Execute(object parameter)
        {
            if (_isNetworkRunning)
            {

            }
            else
            {
                _isNetworkRunning = true;
                Task.Factory.StartNew(() => _viewModel.GenerateNetworks(), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }
    }
}
