using VisualDocker.Infrastructure;

namespace VisualDocker.Controls.Common.CommonFilters
{
    public class CommonFiltersViewModel : NotifyPropertyChangedObject
    {
        private bool _showAll;
        private bool _doNotTruncateResults;

        public bool ShowAll
        {
            get { return _showAll; }
            set { Set(ref _showAll, value); }
        }

        public bool DoNotTruncateResults
        {
            get { return _doNotTruncateResults; }
            set { Set(ref _doNotTruncateResults, value); }
        }
    }
}