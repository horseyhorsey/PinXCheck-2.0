using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.PrismBase;
using Prism.Events;

namespace Hs.PinXCheck.Database.Pane.ViewModels
{
    public class DatabaseOptionViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;

        private bool paneEnabled = true;
        public bool PaneEnabled
        {
            get { return paneEnabled; }
            set { SetProperty(ref paneEnabled, value); }
        }

        public DatabaseOptionViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;

            _eventAggregator.GetEvent<DisableControlsEvent>().Subscribe(b =>
            {
                PaneEnabled = b;
            });
        }
    }
}
