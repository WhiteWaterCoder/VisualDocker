using System.Windows.Controls;

namespace VisualDocker.Controls.Status
{
    public partial class StatusView : UserControl
    {
        public StatusView()
        {
            InitializeComponent();

            DataContext = new StatusViewModel();
        }
    }
}