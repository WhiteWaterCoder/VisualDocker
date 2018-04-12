using VisualDocker.Infrastructure;

namespace VisualDocker.Controls.Common.CommonFilters
{
    public class CommonFiltersViewModel : NotifyPropertyChangedObject
    {
        private bool _showAll;

        public bool ShowAll
        {
            get { return _showAll; }
            set { Set(ref _showAll, value); }
        }
    }
}