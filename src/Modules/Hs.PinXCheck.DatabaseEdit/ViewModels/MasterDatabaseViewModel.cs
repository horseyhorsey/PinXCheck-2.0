using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
using Hs.PinXCheck.Domain.Model;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Hs.PinXCheck.Database.View.ViewModels
{
    public class MasterDatabaseViewModel : ViewModelBase
    {
        #region Fields
        private ITablesRepo _tablesRepo;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private ISelectedService _selectedService;
        #endregion

        #region Constructors
        public MasterDatabaseViewModel(ITablesRepo tableRepo, IRegionManager regionManager, IEventAggregator ea, ISelectedService selectedService)
        {
            _tablesRepo = tableRepo;
            _regionManager = regionManager;
            _eventAggregator = ea;
            _selectedService = selectedService;

            if (_tablesRepo.MasterTableList == null)
            {
                _tablesRepo.MasterTableList = new MasterTables();
                _tablesRepo.GetMasterTables();

                MasterTables = new ListCollectionView(_tablesRepo.MasterTableList);
            }

            SetNewDescriptionCommand = new DelegateCommand(SetNewDescription);

            ExitViewCommand = new DelegateCommand(() =>
            {
                ExitView();
            });

        }
        #endregion

        #region Properties
        private ICollectionView masterTables;
        public ICollectionView MasterTables
        {
            get { return masterTables; }
            set { SetProperty(ref masterTables, value); }
        }

        private string selectedTableInfo;
        public string SelectedTableInfo
        {
            get { return selectedTableInfo; }
            set { SetProperty(ref selectedTableInfo, value); }
        }

        private string _filterText;
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                SetProperty(ref _filterText, value);

                this.MasterTables.Filter = x =>
                {
                    var g = x as PinballTable;

                    if (string.IsNullOrWhiteSpace(_filterText))
                        return true;

                    if (g.Description.ToLower().Contains(_filterText.ToLower()))
                        return true;

                    return false;
                };
            }
        }
        #endregion

        #region Commands
        public DelegateCommand SetNewDescriptionCommand { get; private set; }
        public DelegateCommand ExitViewCommand { get; private set; }
        #endregion

        #region Support Methods
        private void SetNewDescription()
        {
            var currentItem = MasterTables.CurrentItem as IpdbDatabase;

            var tableToEdit = _tablesRepo.PinballXTableList.Where(x => x.Name == _selectedService.SelectedTableName).FirstOrDefault();

            tableToEdit.Description = currentItem.Description;
            tableToEdit.Year = currentItem.Year;
            tableToEdit.Type = currentItem.Type;
            tableToEdit.Rating = currentItem.Rating;
            tableToEdit.Manufacturer = currentItem.Manufacturer;

            _eventAggregator.GetEvent<RefreshMainDatabaseEvent>().Publish("");

            ExitView();
        }

        private void ExitView()
        {
            _eventAggregator.GetEvent<DisableControlsEvent>().Publish(true);

            _regionManager.RequestNavigate(RegionNames.ContentRegion, "DatabaseView");
        }
        #endregion
    }
}
