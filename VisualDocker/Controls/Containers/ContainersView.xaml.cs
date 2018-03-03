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

        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }
    }
}