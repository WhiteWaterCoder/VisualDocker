using DockerCliWrapper.Docker.Container;
using DockerCliWrapper.Docker.Events;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using VisualDocker.Controls.Common.CommonFilters;
using VisualDocker.Extensions;
using VisualDocker.Infrastructure;
using VisualDocker.Models;
using VisualDocker.Services;

namespace VisualDocker.Controls.Containers
{
    public class ContainersViewModel : NotifyPropertyChangedObject
    {
        private readonly EventsStreamer _eventStreamer;
        private readonly DockerContainers _dockerContainers;

        private ObservableCollection<DockerContainerModel> _containers;
        private ICollectionView _containersView;
        private CommonFiltersViewModel _commonFiltersViewModel;
        
        public ICollectionView ContainersView
        {
            get { return _containersView; }
            set { Set(ref _containersView, value); }
        }

        public CommonFiltersViewModel CommonFiltersViewModel
        {
            get { return _commonFiltersViewModel; }
            set { Set(ref _commonFiltersViewModel, value); }
        }
        
        public ContainersViewModel()
        {
            _eventStreamer = EventStreamerSingleton.Instance;
            _dockerContainers = new DockerContainers();

            CommonFiltersViewModel = new CommonFiltersViewModel();
            CommonFiltersViewModel.PropertyChanged += CommonFiltersViewModel_PropertyChanged;

            _containers = new ObservableCollection<DockerContainerModel>();
            _containersView = CollectionViewSource.GetDefaultView(_containers);
            _containersView.Filter += ContainersFilter;

            Initialize();
        }

        private void CommonFiltersViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CommonFiltersViewModel.ShowAll))
            {
                ContainersView.Refresh();
            }
        }

        private bool ContainersFilter(object obj)
        {
            var container = obj as DockerContainerModel;
            if (container == null)
            {
                return false;
            }

            return CommonFiltersViewModel.ShowAll ? true : container.IsRunning;
        }

        private void Initialize()
        {
            var scheduler = DispatcherScheduler.Current;

            Task.Factory
                .StartNew(() =>
                {
                    return _dockerContainers.ShowAll(true)
                                                  .SearchAsync()
                                                  .GetAwaiter()
                                                  .GetResult();
                })
                .ContinueWith(results => 
                {
                    if (results.IsFaulted)
                    {
                        return;
                    }

                    foreach(var c in results.Result)
                    {
                        var container = new DockerContainerModel(c.Id, c.Image, c.Command, c.Created, c.Status, c.Ports, c.Names, c.Size);

                        _containers.Add(container);
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());

            _eventStreamer.GetContainerEventsObservable()
                                  .ObserveOn(DispatcherScheduler.Current)
                                  .Subscribe(async e => await HandleContainerEvent(e));
        }

        private async Task HandleContainerEvent(ContainerEvent e)
        {
            switch (e.EventStatus)
            {
                case ContainerEventStatus.Start:
                case ContainerEventStatus.Unpause:
                    var startedContainer = await GetContainer(e.ShortId, true);
                    startedContainer.Status = ContainerEventStatus.Start;
                    break;

                case ContainerEventStatus.Stop:
                case ContainerEventStatus.Kill:
                case ContainerEventStatus.Die:
                    var stoppedContainer = await GetContainer(e.ShortId, true);
                    stoppedContainer.Status = ContainerEventStatus.Stop;
                    break;

                case ContainerEventStatus.Create:
                    await GetContainer(e.ShortId, true);
                    break;

                case ContainerEventStatus.Destroy:
                    var destroyedContainer = await GetContainer(e.ShortId, false);
                    if (destroyedContainer != null)
                    {
                        _containers.Remove(destroyedContainer);
                    }
                    break;

                case ContainerEventStatus.Pause:
                    var pausedContainer = await GetContainer(e.ShortId, true);
                    pausedContainer.Status = ContainerEventStatus.Pause;
                    break;
            }
        }

        private async Task<DockerContainerModel> GetContainer(string shortId, bool searchAndAddIfNotExists)
        {
            var container = _containers.FirstOrDefault(c => c.Id.EqualsLoose(shortId));

            if (container == null && searchAndAddIfNotExists)
            {
                var containers = await _dockerContainers.ShowAll(true)
                                                        .DoNotTruncate(false)
                                                        .SearchAsync();

                var match = containers.FirstOrDefault(c => c.Id.EqualsLoose(shortId));

                if (match != null)
                {
                    container = new DockerContainerModel(match.Id, match.Image, match.Command, match.Created, match.Status, match.Ports, match.Names, match.Size);
                    _containers.Add(container);
                }
                // else maybe the container was removed so not much can be done
                container = DockerContainerModel.Empty;
            }

            return container;
        }
    }
}