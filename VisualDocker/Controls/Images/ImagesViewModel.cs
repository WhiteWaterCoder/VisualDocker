using DockerCliWrapper.Docker.Images;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using VisualDocker.Controls.Common.CommonFilters;
using VisualDocker.Infrastructure;
using VisualDocker.Models;

namespace VisualDocker.Controls.Images
{
    public class ImagesViewModel : NotifyPropertyChangedObject
    {
        private readonly DockerImages _dockerImages;

        private ObservableCollection<DockerImageModel> _images;
        private CommonFiltersViewModel _commonFiltersViewModel;

        public ObservableCollection<DockerImageModel> Images
        {
            get { return _images; }
            set { Set(ref _images, value); }
        }

        public CommonFiltersViewModel CommonFiltersViewModel
        {
            get { return _commonFiltersViewModel; }
            set { Set(ref _commonFiltersViewModel, value); }
        }

        public ImagesViewModel()
        {
            _dockerImages = new DockerImages();

            CommonFiltersViewModel = new CommonFiltersViewModel();
            CommonFiltersViewModel.PropertyChanged += CommonFiltersViewModel_PropertyChanged;

            FireAndForgetSearch();
        }

        private void CommonFiltersViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CommonFiltersViewModel.ShowAll))
            {
                _dockerImages.ShowAll(CommonFiltersViewModel.ShowAll);
            }

            FireAndForgetSearch();
        }

        private void FireAndForgetSearch()
        {
            Task.Factory.StartNew(async () => 
            {
                var result = await _dockerImages.SearchAsync();

                Images = new ObservableCollection<DockerImageModel>(
                    result.Select(i => new DockerImageModel(i.Id, i.Repository, i.Tag, i.Digest, i.CreatedSince, i.CreatedAt, i.Size)));
            });
        }
    }
}