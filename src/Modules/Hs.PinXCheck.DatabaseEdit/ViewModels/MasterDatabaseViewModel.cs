using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Hs.PinXCheck.Database.View.ViewModels
{
    public class MasterDatabaseViewModel : ViewModelBase
    {
        private ITablesRepo _tablesRepo;
        private IRegionManager _regionManager;

        private ICollectionView masterTables;
        public ICollectionView MasterTables
        {
            get { return masterTables; }
            set { SetProperty(ref masterTables, value); }
        }

        public DelegateCommand SetNewDescriptionCommand { get; private set; } 

        private string selectedTableInfo;
        private IEventAggregator _eventAggregator;
        private ISelectedService _selectedService;

        public string SelectedTableInfo
        {
            get { return selectedTableInfo; }
            set { SetProperty(ref selectedTableInfo, value); }
        }

        public MasterDatabaseViewModel(ITablesRepo tableRepo, IRegionManager regionManager, IEventAggregator ea, ISelectedService selectedService)
        { 
            _tablesRepo = tableRepo;
            _regionManager = regionManager;
            _eventAggregator = ea;
            _selectedService = selectedService;
            
            if (_tablesRepo.MasterTableList == null)
            {
                _tablesRepo.MasterTableList = new VirtualPin.Database.MasterTables();
                _tablesRepo.GetMasterTables();

                MasterTables = new ListCollectionView(_tablesRepo.MasterTableList);
            }

            SetNewDescriptionCommand = new DelegateCommand(SetNewDescription);

        }

        private void SetNewDescription()
        {
            var currentItem = MasterTables.CurrentItem as VirtualPin.Database.IpdbDatabase;            

            var tableToEdit = _tablesRepo.PinballXTableList.Where(x => x.Name == _selectedService.SelectedTableName).FirstOrDefault();

            tableToEdit.Description = currentItem.Description;
            tableToEdit.Year = currentItem.Year;
            tableToEdit.Type = currentItem.Type;
            tableToEdit.Rating = currentItem.Rating;
            tableToEdit.Manufacturer = currentItem.Manufacturer;

            _eventAggregator.GetEvent<RefreshMainDatabaseEvent>().Publish("");
            _eventAggregator.GetEvent<DisableControlsEvent>().Publish(true);

            _regionManager.RequestNavigate(RegionNames.ContentRegion, "DatabaseView");
        }
    }
}
