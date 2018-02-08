using DockerCliWrapper.Docker.Status;
using System.Threading.Tasks;
using VisualDocker.Infrastructure;

namespace VisualDocker.Controls.Status
{
    public class StatusViewModel : NotifyPropertyChangedObject
    {
        private readonly DockerStatus _status;

        private bool _isConnected;
        private string _error;

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

        public StatusViewModel()
        {
            _status = new DockerStatus();

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