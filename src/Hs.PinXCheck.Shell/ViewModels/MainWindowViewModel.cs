using System;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.Events;

namespace Hs.PinXCheck.Shell.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
               
        #region Properties
        private string _title = "PinXCheck 2.0";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion

        #region Commands        
        public DelegateCommand<string> NavigateCommand { get; private set; }
        #endregion

        #region Events
        private IEventAggregator _eventAggregator;
        #endregion

        private IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        #region Methods
        private void Navigate(string uri)
        {
            if (uri == "MasterDatabaseView")
            {                
                _eventAggregator.GetEvent<DisableControlsEvent>().Publish(false);                
            }
            else
                _eventAggregator.GetEvent<DisableControlsEvent>().Publish(true);

            _regionManager.RequestNavigate(RegionNames.ContentRegion, uri);            
        }
        #endregion

    }
}
