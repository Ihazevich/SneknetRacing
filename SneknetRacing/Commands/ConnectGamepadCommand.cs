﻿using SneknetRacing.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SneknetRacing.Commands
{
    class ConnectGamepadCommand : ICommand
    {
        private MainViewModel _viewModel;

        public ConnectGamepadCommand(MainViewModel viewModel)
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
            if (_viewModel.GamepadConnected)
            {
                _viewModel.Controller.Disconnect();
                _viewModel.GamepadConnected = false;
            }
            else
            {
                _viewModel.Controller.Connect();
                _viewModel.GamepadConnected = true;
            }
        }
    }
}
