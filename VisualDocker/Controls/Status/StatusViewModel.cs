using DockerCliWrapper.Docker.Status;
using System.Threading.Tasks;
using System.Windows.Input;
using VisualDocker.Infrastructure;

namespace VisualDocker.Controls.Status
{
    public class StatusViewModel : NotifyPropertyChangedObject
    {
        private readonly DockerStatus _status;

        private bool _isConnected;
        private string _error;

        private ICommand _retryCommand;

        public bool IsConnected
        {
            get { return _isConnected; }
            set { Set(ref _isConnected, value); }
        }

        public string Error
        {
            get { return _error; }
            set { Set(ref _error, value); }
        }

        public ICommand RetryCommand
        {
            get { return _retryCommand; }
            set { Set(ref _retryCommand, value); }
        }

        public StatusViewModel()
        {
            _status = new DockerStatus();

            Connect();

            RetryCommand = new RelayCommand(_ => !IsConnected, _ => Connect());
        }

        private void Connect()
        {
            Task.Factory
                .StartNew(async () =>
                {
                    await _status.Connect();

                    IsConnected = _status.ServerDetails.IsRunning;
                    Error = _status.ServerDetails.Error;
                });
        }
    }
}