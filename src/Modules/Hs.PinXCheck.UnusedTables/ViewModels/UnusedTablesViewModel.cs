using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
using Hs.PinXCheck.UnusedTables.Models;
using Hs.PinXCheck.UnusedTables.Services;
using Hs.VirtualPin.Database;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace Hs.PinXCheck.UnusedTables.ViewModels
{
    public class UnusedTablesViewModel : ViewModelBase
    {
        #region Services
        private IEventAggregator _eventAggregator;
        private ISelectedService _selectedSrv;
        private ITablesRepo _tablesRepo;
        #endregion

        #region Commands
        public DelegateCommand<IList> SelectionChanged { get; private set; }
        public DelegateCommand<string> AddTablesCommand { get; private set; }
        public DelegateCommand ReplaceTableCommand { get; private set; }
        public DelegateCommand OpenFolderCommand { get; private set; } 

        #endregion

        #region Properties
        private TableFiles unusedTables;
        private IList selectedTables;

        private ICollectionView unusedTableList;       
        public ICollectionView UnusedTableList
        {
            get { return unusedTableList; }
            set { SetProperty(ref unusedTableList, value); }
        }

        private UnusedTableOption _unusedTableOptions = new UnusedTableOption();
        public UnusedTableOption UnusedTableOptions
        {
            get { return _unusedTableOptions; }
            set { SetProperty(ref _unusedTableOptions, value); }
        }
        #endregion

        public UnusedTablesViewModel(IEventAggregator ea, ISelectedService selected, ITablesRepo tableRepo)
        {
            _eventAggregator = ea;
            _selectedSrv = selected;
            _tablesRepo = tableRepo;

            if (_tablesRepo.PinballXTableList == null)
                _tablesRepo.PinballXTableList = new PinballXTables();

            unusedTables = new TableFiles();
            UnusedTableList = new ListCollectionView(unusedTables);

            SelectionChanged = new DelegateCommand<IList>(items =>
            {
                selectedTables = new List<TableFile>();

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        selectedTables.Add(item as TableFile);
                    }
                }

            });

            AddTablesCommand = new DelegateCommand<string>(AddTables);

            ReplaceTableCommand = new DelegateCommand(ReplaceTable);

            _eventAggregator.GetEvent<AddToUnusedTablesEvent>().Subscribe(x =>
            {
                unusedTables.Add(new TableFile()
                {
                    TableName = x
                });
            });

            OpenFolderCommand = new DelegateCommand(() =>
            {
                Process.Start(_selectedSrv.CurrentTablePath);
            }, () => !string.IsNullOrWhiteSpace(_selectedSrv.CurrentTablePath));

            _eventAggregator.GetEvent<UpdatedUnusedTables>().Subscribe(GetUnusedTables);

            //GetUnusedTables("");
        }

        #region Methods
        private void ReplaceTable()
        {
            try
            {
                var tableFile = UnusedTableList.CurrentItem as TableFile;

                unusedTables.Remove(tableFile);

                _eventAggregator.GetEvent<ReplaceTableEvent>().Publish(tableFile.TableName);
            }
            catch (Exception) { }
            
        }

        private void AddTables(string tableAddType)
        {
            switch (tableAddType)
            {
                case "SelectedTables":
                    AddSelectedTables();
                    break;
                case "AllTables":
                    AddAllTables();
                    break;
                default:
                    break;
            }
        }

        private void AddSelectedTables()
        {
            if (selectedTables != null)
            {
                foreach (TableFile item in selectedTables)
                {
                    _tablesRepo.PinballXTableList.Add(new PinballXTable()
                    {
                        Name = item.TableName,
                        Description = item.TableName,
                        Enabled = UnusedTableOptions.Enabled,
                        HideDmd = UnusedTableOptions.Dmd,
                        HideBackGlass = UnusedTableOptions.Translite
                    });

                    unusedTables.Remove(item);
                }
            }
        }

        private void AddAllTables()
        {
            try
            {
                foreach (var table in unusedTables)
                {
                    _tablesRepo.PinballXTableList.Add(new PinballXTable()
                    {
                        Name = table.TableName,
                        Description = table.TableName,
                        Enabled = UnusedTableOptions.Enabled,
                        HideDmd = UnusedTableOptions.Dmd,
                        HideBackGlass = UnusedTableOptions.Translite
                    });
                }

                unusedTables.Clear();
            }
            catch (Exception) { }

        }

        private void GetUnusedTables(string tableFolder)
        {
            try
            {
                var tablesPath = _selectedSrv.CurrentTablePath;

                var getFileService = new GetFilesService();

                var extensions = getTableExtensionForSystem();

                var tableFiles = getFileService.GetAllFilesInDirectory(tablesPath);

                unusedTables.Clear();

                foreach (var item in tableFiles)
                {
                    var extension = item.Extension;

                    if (extensions.Contains(extension))
                    {
                        var tableName = Path.GetFileNameWithoutExtension(item.FullName);


                        try
                        {
                            var tableExistsInDb =
                                _tablesRepo.PinballXTableList.Where(x => x.Name == tableName);
                            
                            if (tableExistsInDb.Count() == 0)
                            {
                                unusedTables.Add(new TableFile()
                                {
                                    TableName = tableName,
                                    Extension = item.Extension,
                                    TableDate = item.LastWriteTime,
                                    TableFileName = item.FullName
                                });
                            }
                        }
                        catch (Exception)
                        {
                            
                        }

                    }
                }
            }
            catch (Exception) { }
          
        }

        private string[] getTableExtensionForSystem()
        {
            var extensions = new string[2];

            var systemType = _selectedSrv.CurrentSystemType;

            switch (systemType)
            {
                case 1:
                    extensions[0] = ".vpt";
                    extensions[1] = ".vpx";
                    break;
                case 2:
                    extensions[0] = ".fpt";
                    extensions[1] = "";
                    break;
                default:
                    break;
            }

            return extensions;
        }
        #endregion
       
    }
}
