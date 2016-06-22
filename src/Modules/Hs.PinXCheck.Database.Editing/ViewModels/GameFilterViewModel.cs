using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.PrismBase;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Hs.PinXCheck.Database.Editing.ViewModels
{
    public class GameFilterViewModel : ViewModelBase
    {
        private bool enabledFilter;
        public bool EnabledFilter
        {
            get { return enabledFilter; }
            set { SetProperty(ref enabledFilter, value); }
        }

        private bool unmatchedFilter;
        public bool UnmatchedFilter
        {
            get { return unmatchedFilter; }
            set { SetProperty(ref unmatchedFilter, value); }
        }

        private string textFilter;
        public string TextFilter
        {
            get { return textFilter; }
            set {
                SetProperty(ref textFilter, value);
                OnPropertyChanged(TextFilter);
            }
        }

        private IEventAggregator _eventAggregator;

        public GameFilterViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            var filterOptions = new List<object>();

            filterOptions.Add(EnabledFilter);
            filterOptions.Add(UnmatchedFilter);
            filterOptions.Add(TextFilter);

            _eventAggregator.GetEvent<FilterEvent>().Publish(filterOptions);
        }
    }
}
