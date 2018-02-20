using VisualDocker.Infrastructure;

namespace VisualDocker.Models
{
    public class DockerContainerModel : NotifyPropertyChangedObject
    {
        private string _id;
        private string _image;
        private string _command;
        private string _created;
        private string _status;
        private string _ports;
        private string _names;
        private string _size;

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
            private set { Set(ref _status, value); }
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

        public DockerContainerModel(string id, string image, string command, string created, string status, string ports, string names, string size)
        {
            Id = id;
            Image = image;
            Command = command;
            Created = created;
            Status = status;
            Ports = ports;
            Names = names;
            Size = size;
        }

        public override string ToString()
        {
            return Image;
        }
    }
}
