using System.Windows.Controls;

namespace VisualDocker.Controls.Containers
{
    public partial class ContainersView : UserControl
    {
        public ContainersView()
        {
            InitializeComponent();

            DataContext = new ContainersViewModel();
        }
    }
}