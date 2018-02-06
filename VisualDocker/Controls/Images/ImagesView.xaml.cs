using System.Windows.Controls;

namespace VisualDocker.Controls.Images
{
    public partial class ImagesView : UserControl
    {
        public ImagesView()
        {
            InitializeComponent();

            DataContext = new ImagesViewModel();
        }
    }
}
