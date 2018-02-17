using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.PrismBase;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Hs.PinXCheck.Base.Interfaces;
using System.Windows.Data;
using System.ComponentModel;
using Hs.PinXCheck.Base.Services;
using Hs.VirtualPin.Database;
using Prism.Regions;
using Hs.PinXCheck.Base.Constants;
using System.Collections;
using Hs.Services.VisualPinball;
using System.IO;

namespace Hs.PinXCheck.Database.View.ViewModels
{
    public class DatabaseViewModel : ViewModelBase
    {
        #region Services
        private IEventAggregator _eventAggregator;
        private ITablesRepo _tableRepo;
        private ISelectedService _selectedService;
        private ISettingsRepo _settings;
        private IRegionManager _regionManager;
        #endregion

        #region Filter Properties
        public string TextFilter { get; set; }
        public bool EnabledFilter { get; set; }
        public bool Unmatched { get; set; }
        #endregion

        #region Properties
        private ICollectionView tableList;
        public ICollectionView TableList
        {
            get { return tableList; }
            set
            {
                SetProperty(ref tableList, value);
            }
        }

        List<PinballXTable> SelectedGames { get; set; }

        public int SelectedItemsCount { get; private set; }

        private string currentColumn;
        #endregion

        #region Commands
        public DelegateCommand RemoveTableCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand<IList> SelectionChanged { get; set; }
        public DelegateCommand<object> RowEditEndedCommand { get; private set; }

        #endregion

        #region Constructors
        public DatabaseViewModel(IEventAggregator ea, ITablesRepo tableRepo, 
             ISelectedService selectedService, IRegionManager regionManager
            , ISettingsRepo settings)
        {
            _eventAggregator = ea;
            _regionManager = regionManager;
            _tableRepo = tableRepo;
            _selectedService = selectedService;
            _settings = settings;

            TableList = new ListCollectionView(_tableRepo.PinballXTableList);

            SelectedGames = new List<PinballXTable>();

            _eventAggregator.GetEvent<DatabaseChanged>().Subscribe(UpdateCurrentDatabase);
            _eventAggregator.GetEvent<ReplaceTableEvent>().Subscribe(ReplaceTable);
            _eventAggregator.GetEvent<ReplaceExecutableEvent>().Subscribe(UpdateExecutable);
            _eventAggregator.GetEvent<GetTableInfoEvent>().Subscribe(GetTableInfo);
            _eventAggregator.GetEvent<SetExtraTableOptionsEvent>().Subscribe(SetTableOptions);

            RemoveTableCommand = new DelegateCommand(() =>
            {
                try
                {
                    var table = TableList.CurrentItem as PinballXTable;
                    _tableRepo.PinballXTableList.Remove(table);
                    _eventAggregator.GetEvent<AddToUnusedTablesEvent>().Publish(table.Name);
                }
                catch (Exception) { }
            });

            NavigateCommand = new DelegateCommand<string>(x =>
            {
                var selectedTable = TableList.CurrentItem as PinballXTable;
                _regionManager.RequestNavigate(RegionNames.ContentRegion, x);
                _eventAggregator.GetEvent<DisableControlsEvent>().Publish(false);

            });

            _eventAggregator.GetEvent<FilterEvent>().Subscribe(SetFilter);

            // Command for datagrid selectedItems
            SelectionChanged = new DelegateCommand<IList>(
                items =>
                {
                    if (items == null)
                    {
                        SelectedItemsCount = 0;
                        SelectedGames.Clear();
                        return;
                    }
                    else
                    {
                        SelectedItemsCount = items.Count;
                    }

                    try
                    {
                        SelectedGames.Clear();
                        foreach (var table in items)
                        {
                            var game = table as PinballXTable;
                            if (game.Name != null)
                                SelectedGames.Add(game);
                        }

                        //if (SelectedItemsCount > 1)
                        //    DatabaseHeaderInfo = "Selected items: " + SelectedItemsCount;
                        //else if (SelectedItemsCount == 1)
                        //{
                        //    var game = items[0] as Game;
                        //    DatabaseHeaderInfo = "Selected item: " + game.RomName;
                        //}
                        //else
                        //    DatabaseHeaderInfo = "";
                    }
                    catch (Exception) { }

                });
            
            RowEditEndedCommand = new DelegateCommand<object>(RowEditEnded);

            //Why 2 events??
            _eventAggregator.GetEvent<DescriptionUpdatedEvent>().Subscribe(RefreshTables);
            _eventAggregator.GetEvent<RefreshMainDatabaseEvent>().Subscribe(RefreshTables);      

        }

        #endregion

        #region Methods
        private void RowEditEnded(object obj)
        {
            var dataGrid = obj as System.Windows.Controls.DataGrid;

            currentColumn = dataGrid.CurrentColumn.Header.ToString();
            var table = dataGrid.CurrentItem as PinballXTable;
            
            if (currentColumn == "Description")
            {

                TableList.CurrentChanged -= TableList_CurrentChanged;                

                var matched = _tableRepo.MatchDescription(table.Description);

                var index = _tableRepo.PinballXTableList.IndexOf(table);

                _tableRepo.PinballXTableList[index].IsDescriptionMatched = matched;                
                    
                // WHYYYYYYYYYYYYYY>>>??? :-1:
                TableList = new ListCollectionView(_tableRepo.PinballXTableList);

                if (!matched)
                {                    
                    _tableRepo.UnMatchedTableList.Add(new UnMatchedTable()
                    {
                        Description = table.Description,
                        Year = table.Year,
                        FileName = table.Name,
                        FlagRename = false,                        

                    });
                }

                TableList.CurrentChanged += TableList_CurrentChanged;
            }
            else if (currentColumn == "Table File")
            {
               table.TableFileExists = _tableRepo.GetTableFileName(Path.Combine(_selectedService.CurrentTablePath, table.Name));                
            }
        }

        private void RefreshTables(string obj)
        {
            try
            {
                TableList.Refresh();
            }
            catch (Exception) { }

        }

        private async void UpdateCurrentDatabase(string databasePath)
        {            
            TableList = new ListCollectionView(_tableRepo.PinballXTableList);

            await _tableRepo.GetTablesFromXmlAsync(databasePath, _selectedService.CurrentTablePath);
           
            FinishedUpdatingDatabase();

        }

        private void FinishedUpdatingDatabase()
        {
            TableList.CurrentChanged += TableList_CurrentChanged;

            _eventAggregator.GetEvent<DatabaseUpdated>().Publish("");
            _eventAggregator.GetEvent<UpdatedUnusedTables>().Publish("");
        }

        private void TableList_CurrentChanged(object sender, EventArgs e)
        {
            if (TableList != null)
            {
                var table = TableList.CurrentItem as PinballXTable;
                if (table != null)
                {
                    _selectedService.SelectedTableName = table.Name;
                    _selectedService.SelectedDescription = table.Description;
                    _selectedService.SelectedPublisher = table.Manufacturer;

                    var systemMediaDirectory = _settings.PinXCheckSettings.PinballXPath +
                        "//Media//"; 
                    _eventAggregator.GetEvent<TableSelectedEvent>().Publish(systemMediaDirectory);
                }
            }
        }

        private void SetFilter(List<object> filterOptions)
        {
            EnabledFilter = (bool)filterOptions.ElementAt(0);
            Unmatched = (bool)filterOptions.ElementAt(1);
            TextFilter = (string)filterOptions.ElementAt(2);

            var cv = CollectionViewSource.GetDefaultView(TableList);

            if (EnabledFilter || Unmatched || !string.IsNullOrEmpty(TextFilter))
            {
                cv.Filter = o =>
                {
                    var g = o as PinballXTable;
                    var textFiltered = false;

                    if (string.IsNullOrEmpty(TextFilter))
                    {
                        if (Unmatched)
                        {
                            textFiltered = g.IsDescriptionMatched.Equals(false);
                        }
                        else if (EnabledFilter)
                        {
                            textFiltered =
                            g.Enabled.Equals(!EnabledFilter);
                        }

                    }
                    else
                    {

                        if (Unmatched)
                        {
                            textFiltered =
                            textFiltered = g.Description.ToUpper().Contains(TextFilter.ToUpper()) &&
                            g.IsDescriptionMatched.Equals(false);
                        }
                        else if (EnabledFilter)
                        {
                            textFiltered =
                            textFiltered = g.Description.ToUpper().Contains(TextFilter.ToUpper()) &&
                            g.Enabled.Equals(EnabledFilter);
                        }
                        else if (!EnabledFilter && !Unmatched)
                        {
                            textFiltered =
                            textFiltered = g.Description.ToUpper().Contains(TextFilter.ToUpper());
                        }
                    }

                    return textFiltered;
                };
            }
            else
            {
                cv.Filter = null;
            }
        }

        private void SetTableOptions(Dictionary<string, bool> extraOption)
        {
            foreach (var table in SelectedGames)
            {
                if (extraOption.ElementAt(0).Key == "Enabled")
                    table.Enabled = extraOption["Enabled"];
                else if (extraOption.ElementAt(0).Key == "HideDmd")
                    table.HideDmd = extraOption["HideDmd"];
                else if (extraOption.ElementAt(0).Key == "HideBackglass")
                    table.HideBackGlass = extraOption["HideBackglass"];
            }

            TableList.Refresh();
        }

        private void GetTableInfo(string infoType)
        {
            var visualPinballInfo = new VisualPinball();

            VisualPinball.SevenZipPath =
                _settings.PinXCheckSettings.PinballXPath + "\\" + VisualPinball.SevenZipExe;

            foreach (var table in SelectedGames)
            {
                var tableFile = _selectedService.CurrentTablePath + "\\" + table.Name;

                var setName = "";

                //Only set romname for visual pinball tables
                switch (infoType)
                {
                    case "Rom":
                        if (_selectedService.CurrentSystemType == 1)
                        {
                            setName = visualPinballInfo.GetInfoVisualPinball(_selectedService.CurrentSystemType,
                               table.Name, _selectedService.CurrentTablePath, infoType);

                            setRomName(setName, table.Name);
                        }
                        break;
                    case "Author":
                        if (_selectedService.CurrentSystemType == 1)
                        {
                            setName = visualPinballInfo.GetInfoVisualPinball(_selectedService.CurrentSystemType,
                               table.Name, _selectedService.CurrentTablePath, infoType);

                            setAuthorName(setName, table.Name);
                        }
                        else if (_selectedService.CurrentSystemType == 2)
                        {
                            setName = visualPinballInfo.GetInfoFuturePinball(_selectedService.CurrentSystemType,
                                table.Name, _selectedService.CurrentTablePath);

                            setAuthorName(setName, table.Name);
                        }
                        break;
                    default:
                        break;
                }

            }

            TableList.Refresh();

        }

        private void setRomName(string setName, string tableName)
        {
            if (!string.IsNullOrEmpty(setName))
            {
                _tableRepo.PinballXTableList.Where(x => x.Name == tableName).FirstOrDefault()
                    .RomName = setName;
            }
        }

        private void setAuthorName(string setName, string tableName)
        {
            if (!string.IsNullOrEmpty(setName))
            {
                _tableRepo.PinballXTableList.Where(x => x.Name == tableName).FirstOrDefault()
                    .Author = setName;
            }
        }

        private void refreshTables()
        {

        }

        private void ReplaceTable(string tableFileName)
        {
            try
            {
                var currentTable = TableList.CurrentItem as PinballXTable;

                _tableRepo.PinballXTableList.Where(x => x.Name == currentTable.Name)
                    .First()
                    .Name = tableFileName;

                TableList.Refresh();
            }
            catch (Exception) { }

        }

        private void UpdateExecutable(string executable)
        {
            try
            {
                var currentTable = TableList.CurrentItem as PinballXTable;

                foreach (var table in SelectedGames)
                {
                    _tableRepo.PinballXTableList.Where(x => x.Name == table.Name)
                    .First()
                    .AlternateExe = executable;
                }

                TableList.Refresh();

            }
            catch (Exception) { }
        }

        #endregion

    }
}
