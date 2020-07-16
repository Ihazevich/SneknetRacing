using SneknetRacing.Models;
using SneknetRacing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SneknetRacing.Commands
{
    public class UpdateMotionViewCommand : ICommand
    {
        private MainViewModel _viewModel;

        public UpdateMotionViewCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.MotionDataViewModel.SelectedCarMotionData = ((PacketMotionData)_viewModel.MotionDataViewModel.Packet).CarMotionData.ElementAt(int.Parse(parameter.ToString()));
        }
    }
}
