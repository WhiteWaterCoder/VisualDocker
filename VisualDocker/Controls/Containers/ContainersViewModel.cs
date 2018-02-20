using DockerCliWrapper.Docker.Container;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using VisualDocker.Controls.Common.CommonFilters;
using VisualDocker.Infrastructure;
using VisualDocker.Models;

namespace VisualDocker.Controls.Containers
{
    public class ContainersViewModel : NotifyPropertyChangedObject
    {
        private readonly DockerContainers _dockerContainers;

        private ObservableCollection<DockerContainerModel> _containers;
        private CommonFiltersViewModel _commonFiltersViewModel;

        public ObservableCollection<DockerContainerModel> Containers
        {
            get { return _containers; }
            set { Set(ref _containers, value); }
        }

        public CommonFiltersViewModel CommonFiltersViewModel
        {
            get { return _commonFiltersViewModel; }
            set { Set(ref _commonFiltersViewModel, value); }
        }

        public ContainersViewModel()
        {
            _dockerContainers = new DockerContainers();

            CommonFiltersViewModel = new CommonFiltersViewModel();
            CommonFiltersViewModel.PropertyChanged += CommonFiltersViewModel_PropertyChanged;

            FireAndForgetSearch(true);
        }

        private void CommonFiltersViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CommonFiltersViewModel.ShowAll))
            {
                _dockerContainers.ShowAll(CommonFiltersViewModel.ShowAll);
            }
            else if (e.PropertyName == nameof(CommonFiltersViewModel.DoNotTruncateResults))
            {
                _dockerContainers.DoNotTruncate(CommonFiltersViewModel.DoNotTruncateResults);
            }

            FireAndForgetSearch(true);
        }

        private void FireAndForgetSearch(bool clearResults)
        {
            Task.Factory.StartNew(async () =>
            {
                if (clearResults)
                {
                    Containers = new ObservableCollection<DockerContainerModel>();
                }

                var result = await _dockerContainers.SearchAsync();

                foreach (var c in result)
                {
                    var container = new DockerContainerModel(c.Id, c.Image, c.Command, c.Created, c.Status, c.Ports, c.Names, c.Size);

                    Containers.Add(container);
                }
            });
        }
    }
}