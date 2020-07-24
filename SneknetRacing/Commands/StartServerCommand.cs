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

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return !_viewModel.NetworkThreadsRunning;
        }

        public void Execute(object parameter)
        {
            _viewModel.CarSetupsDataViewModel.DesserializationThread.Start();
            _viewModel.CarStatusDataViewModel.DesserializationThread.Start();
            _viewModel.CarTelemetryDataViewModel.DesserializationThread.Start();
            _viewModel.ClassificationDataViewModel.DesserializationThread.Start();
            _viewModel.EventDataViewModel.DesserializationThread.Start();
            _viewModel.HeaderViewModel.DesserializationThread.Start();
            _viewModel.LapDataViewModel.DesserializationThread.Start();
            _viewModel.LobbyInfoDataViewModel.DesserializationThread.Start();
            _viewModel.MotionDataViewModel.DesserializationThread.Start();
            _viewModel.ParticipantsDataViewModel.DesserializationThread.Start();
            _viewModel.SessionDataViewModel.DesserializationThread.Start();
            _viewModel.DesserializationThread.Start();
            // Start Server thread
            _viewModel.ServerThread.Start();
            // Start Handler thread
            _viewModel.DataHandlerThread.Start();
            _viewModel.NetworkThreadsRunning = true;
            // Start Serializer thread
            _viewModel.SerializerThread.Start();
        }
    }
}
