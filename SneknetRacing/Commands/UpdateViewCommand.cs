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

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "Header" && _viewModel.SelectedViewModel.GetType() == _viewModel.HeaderViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Motion" && _viewModel.SelectedViewModel.GetType() == _viewModel.MotionDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Session" && _viewModel.SelectedViewModel.GetType() == _viewModel.SessionDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Lap" && _viewModel.SelectedViewModel.GetType() == _viewModel.LapDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Event" && _viewModel.SelectedViewModel.GetType() == _viewModel.EventDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Participants" && _viewModel.SelectedViewModel.GetType() == _viewModel.ParticipantsDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Setups" && _viewModel.SelectedViewModel.GetType() == _viewModel.CarSetupsDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Telemetry" && _viewModel.SelectedViewModel.GetType() == _viewModel.CarTelemetryDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Status" && _viewModel.SelectedViewModel.GetType() == _viewModel.CarStatusDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Classification" && _viewModel.SelectedViewModel.GetType() == _viewModel.ClassificationDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Lobby" && _viewModel.SelectedViewModel.GetType() == _viewModel.LobbyInfoDataViewModel.GetType())
            {
                return false;
            }
            if (parameter.ToString() == "Neural" && _viewModel.SelectedViewModel.GetType() == _viewModel.NeuralDataViewModel.GetType())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            if(parameter.ToString() == "Header")
            {
                _viewModel.SelectedViewModel = _viewModel.HeaderViewModel;
            }
            else if (parameter.ToString() == "Motion")
            {
                _viewModel.SelectedViewModel = _viewModel.MotionDataViewModel;
            }
            else if (parameter.ToString() == "Session")
            {
                _viewModel.SelectedViewModel = _viewModel.SessionDataViewModel;
            }
            else if (parameter.ToString() == "Lap")
            {
                _viewModel.SelectedViewModel = _viewModel.LapDataViewModel;
            }
            else if (parameter.ToString() == "Event")
            {
                _viewModel.SelectedViewModel = _viewModel.EventDataViewModel;
            }
            else if (parameter.ToString() == "Participants")
            {
                _viewModel.SelectedViewModel = _viewModel.ParticipantsDataViewModel;
            }
            else if (parameter.ToString() == "Setups")
            {
                _viewModel.SelectedViewModel = _viewModel.CarSetupsDataViewModel;
            }
            else if (parameter.ToString() == "Telemetry")
            {
                _viewModel.SelectedViewModel = _viewModel.CarTelemetryDataViewModel;
            }
            else if (parameter.ToString() == "Status")
            {
                _viewModel.SelectedViewModel = _viewModel.CarStatusDataViewModel;
            }
            else if (parameter.ToString() == "Classification")
            {
                _viewModel.SelectedViewModel = _viewModel.ClassificationDataViewModel;
            }
            else if (parameter.ToString() == "Lobby")
            {
                _viewModel.SelectedViewModel = _viewModel.LobbyInfoDataViewModel;
            }
            else if (parameter.ToString() == "Neural")
            {
                _viewModel.SelectedViewModel = _viewModel.NeuralDataViewModel;
            }

            _viewModel.Packet = _viewModel.SelectedViewModel.Packet;
        }
    }
}
