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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ParentControl.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}