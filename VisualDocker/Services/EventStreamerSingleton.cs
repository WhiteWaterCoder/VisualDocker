using DockerCliWrapper.Docker.Events;

namespace VisualDocker.Services
{
    public class EventStreamerSingleton
    {
        static EventStreamerSingleton()
        {
            Instance = new EventsStreamer();
        }

        public static EventsStreamer Instance { get; private set; }
    }

}