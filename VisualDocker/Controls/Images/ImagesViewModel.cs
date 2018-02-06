using DockerCliWrapper.Docker.Images;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VisualDocker.Infrastructure;
using VisualDocker.Models;

namespace VisualDocker.Controls.Images
{
    public class ImagesViewModel : NotifyPropertyChangedObject
    {
        private readonly DockerImages _dockerImages;

        private ObservableCollection<DockerImageModel> _images;
        private bool _areResultsTruncated;

        public ObservableCollection<DockerImageModel> Images
        {
            get { return _images; }
            set { Set(ref _images, value); }
        }

        public bool AreResultsTruncated
        {
            get { return _areResultsTruncated; }
            set
            {
                if (Set(ref _areResultsTruncated, value))
                {
                    _dockerImages.DoNotTruncate();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    ExecuteSearch(true);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }

        public ImagesViewModel()
        {
            _dockerImages = new DockerImages();
            
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ExecuteSearch(true);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private async Task ExecuteSearch(bool clearResults)
        {
            if (clearResults)
            {
                Images = new ObservableCollection<DockerImageModel>();
            }

            var result = await _dockerImages.Execute();
            
            foreach (var i in result)
            {
                var image = new DockerImageModel(i.ImageId, i.Repository, i.Tag, i.Digest, i.CreatedSince, i.CreatedAt, i.Size);

                Images.Add(image);
            }
        }
    }
}