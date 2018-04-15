using DockerCliWrapper.Docker.Container;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using VisualDocker.Extensions;
using VisualDocker.Infrastructure;

namespace VisualDocker.Models
{
    public class DockerContainerModel : NotifyPropertyChangedObject
    {
        public static DockerContainerModel Empty { get; } = new DockerContainerModel();

        private readonly DockerContainer _container;

        private string _id;
        private string _image;
        private string _command;
        private string _created;
        private ContainerEventStatus _status;
        private string _ports;
        private string _names;
        private string _size;
        private bool _isRunning;
        private bool _isPaused;

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

        public ContainerEventStatus Status
        {
            get { return _status; }
            set
            {
                if (Set(ref _status, value))
                {
                    IsRunning = Status == ContainerEventStatus.Start;
                    IsPaused = Status == ContainerEventStatus.Pause;
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

        private DockerContainerModel()
        {
        }

        public DockerContainerModel(string id, string image, string command, string created, string status, string ports, string names, string size)
        {
            _container = new DockerContainer(id);

            Id = id;
            Image = image;
            Command = command;
            Created = created;
            Ports = ports;
            Names = names;
            Size = size;

            if (status.StartsWith("Up"))
            {
                Status = status.ContainsLoose("paused") 
                            ? ContainerEventStatus.Pause 
                            : ContainerEventStatus.Start;
            }
            else if (status.ContainsLoose("Restarting"))
            {
                Status = ContainerEventStatus.Restart;
            }
            else if (status.ContainsLoose("Created") || status.ContainsLoose("Exited"))
            {
                Status = ContainerEventStatus.Stop;
            }

            StartCommand = new RelayCommand(_ => !IsRunning, _ => { _container.Start(); });
            StopCommand = new RelayCommand(_ => IsRunning, _ => { _container.Stop(); });
            RestartCommand = new RelayCommand(_ => true, _ => { _container.Restart(); });
            PauseCommand = new RelayCommand(_ => !IsPaused, _ => { _container.Pause(); });
            KillCommand = new RelayCommand(_ => IsRunning, _ => { _container.Kill(); });
        }

        public override string ToString()
        {
            return Image;
        }
    }
}