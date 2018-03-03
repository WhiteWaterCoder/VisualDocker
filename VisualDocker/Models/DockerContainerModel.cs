using DockerCliWrapper.Docker.Container;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using VisualDocker.Infrastructure;

namespace VisualDocker.Models
{
    public class DockerContainerModel : NotifyPropertyChangedObject
    {
        private readonly DockerContainer _container;

        private string _id;
        private string _image;
        private string _command;
        private string _created;
        private string _status;
        private string _ports;
        private string _names;
        private string _size;
        private bool _isRunning;
        private bool _isPaused;
        private bool _awaitingStatusUpdate;

        private ICommand _startCommand;
        private ICommand _stopCommand;
        private ICommand _restartCommand;
        private ICommand _pauseCommand;
        private ICommand _killCommand;

        public string Id
        {
            get { return _id; }
            private set { Set(ref _id, value); }
        }

        public string Image
        {
            get { return _image; }
            private set { Set(ref _image, value); }
        }

        public string Command
        {
            get { return _command; }
            private set { Set(ref _command, value); }
        }

        public string Created
        {
            get { return _created; }
            private set { Set(ref _created, value); }
        }

        public string Status
        {
            get { return _status; }
            private set
            {
                if (Set(ref _status, value))
                {
                    _awaitingStatusUpdate = false;

                    IsRunning = Status.StartsWith("Up");
                    IsPaused = Status.Contains("Paused");
                }
            }
        }

        public string Ports
        {
            get { return _ports; }
            private set { Set(ref _ports, value); }
        }

        public string Names
        {
            get { return _names; }
            private set { Set(ref _names, value); }
        }

        public string Size
        {
            get { return _size; }
            private set { Set(ref _size, value); }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            private set { Set(ref _isRunning, value); }
        }

        public bool IsPaused
        {
            get { return _isPaused; }
            private set { Set(ref _isPaused, value); }
        }

        public ICommand StartCommand
        {
            get { return _startCommand; }
            set { Set(ref _startCommand, value); }
        }

        public ICommand StopCommand
        {
            get { return _stopCommand; }
            set { Set(ref _stopCommand, value); }
        }

        public ICommand RestartCommand
        {
            get { return _restartCommand; }
            set { Set(ref _restartCommand, value); }
        }

        public ICommand PauseCommand
        {
            get { return _pauseCommand; }
            set { Set(ref _pauseCommand, value); }
        }

        public ICommand KillCommand
        {
            get { return _killCommand; }
            set { Set(ref _killCommand, value); }
        }

        public DockerContainerModel(string id, string image, string command, string created, string status, string ports, string names, string size)
        {
            _container = new DockerContainer(id);

            Id = id;
            Image = image;
            Command = command;
            Created = created;
            Status = status;
            Ports = ports;
            Names = names;
            Size = size;

            StartCommand = new RelayCommand(_ => !IsRunning, _ => { _container.Start(); UpdateStatus(); });
            StopCommand = new RelayCommand(_ => IsRunning, _ => { _container.Stop(); UpdateStatus(); });
            RestartCommand = new RelayCommand(_ => true, _ => { _container.Restart(); UpdateStatus(); });
            PauseCommand = new RelayCommand(_ => !IsPaused, _ => { _container.Pause(); UpdateStatus(); });
            KillCommand = new RelayCommand(_ => IsRunning, _ => { _container.Kill(); UpdateStatus(); });
        }

        public override string ToString()
        {
            return Image;
        }

        private void UpdateStatus()
        {
            _awaitingStatusUpdate = true;

            Task.Factory.StartNew(() =>
            {
                var containers = new DockerContainers();
                containers.ShowAll(true);

                while (_awaitingStatusUpdate)
                {
                    var match = containers.SearchAsync()
                                          .GetAwaiter()
                                          .GetResult()
                                          .FirstOrDefault(r => { return string.Equals(r.Id, Id); });

                    if (match != null)
                    {
                        Ports = match.Ports;
                        Status = match.Status;
                    }

                    Thread.Sleep(1000);
                }
            });
        }
    }
}