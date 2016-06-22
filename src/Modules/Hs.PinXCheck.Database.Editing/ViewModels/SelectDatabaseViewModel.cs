using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace Hs.PinXCheck.Database.Editing.ViewModels
{
    public class SelectDatabaseViewModel : ViewModelBase
    {
        
        private IEventAggregator _eventAggregator;
        private ISettingsRepo _settingsRepo;

        private ICollectionView databases;        
        public ICollectionView Databases
        {
            get { return databases; }
            set { SetProperty(ref databases, value); }
        }

        private bool databaseComboBoxEnabled = true;
        public bool DatabaseComboBoxEnabled
        {
            get { return databaseComboBoxEnabled; }
            set { SetProperty(ref databaseComboBoxEnabled, value); }
        }

        public DelegateCommand DatabaseChangedCommand { get; private set; } 

        private string selectedDatabase;
        public string SelectedDatabase
        {
            get { return selectedDatabase; }
            set { SetProperty(ref selectedDatabase, value); }
        }

        private string currentSystem;

        public SelectDatabaseViewModel(IEventAggregator ea, ISettingsRepo settings)
        {
            _eventAggregator = ea;
            _settingsRepo = settings;
           
            Databases = new ListCollectionView(new List<string>());
            
            GetDatabases("Visual Pinball");
            DatabaseChanged();

            DatabaseChangedCommand = new DelegateCommand(DatabaseChanged);

            _eventAggregator.GetEvent<DisableControlsEvent>().Subscribe(b => 
            {
                DatabaseComboBoxEnabled = b;
            });

            _eventAggregator.GetEvent<SystemSelected>().Subscribe(GetDatabases);
        }

        private void DatabaseChanged()
        {
            try
            {
                _eventAggregator.GetEvent<DatabaseChanged>().Publish(_settingsRepo.PinXCheckSettings.PinballXPath + "\\Databases\\" + currentSystem + "\\" + SelectedDatabase);
                _eventAggregator.GetEvent<UpdatedUnusedTables>().Publish("");                
            }
            catch (Exception) { }
            
        }

        private void GetDatabases(string systemName)
        {
            currentSystem = systemName;

            try
            {
                var dbPath = _settingsRepo.PinXCheckSettings.PinballXPath + "\\Databases\\" + systemName;
                var databaseFiles = Directory.GetFiles(dbPath, "*.xml");
                var tempDb = new List<string>();
                
                foreach (var item in databaseFiles)
                {
                    tempDb.Add(Path.GetFileName(item));
                }                

                try
                {                    
                    SelectedDatabase = systemName + ".xml";
                }
                catch (Exception) { }

                Databases = new ListCollectionView(tempDb);

            }
            catch (Exception) { }
            
        }
    }
}
