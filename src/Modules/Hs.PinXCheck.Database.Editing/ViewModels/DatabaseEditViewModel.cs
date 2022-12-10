using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
using Hs.PinXCheck.Database.Editing.Models;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Data;

namespace Hs.PinXCheck.Database.Editing.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseEditViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private ISelectedService _selectedService;
        private ISettingsRepo _settingsRepo;

        #region Commands
        public DelegateCommand<string> ReplaceExeCommand { get; private set; }
        public DelegateCommand<string> GetTableInfoCommand { get; private set; }
        public DelegateCommand<string> SetRadioGroup { get; private set; }
        public DelegateCommand<string> SetOptionsCommand { get; private set; }
        #endregion

        private DbEditOption  _dbEditOption;
        public DbEditOption DbEditOption
        {
            get { return _dbEditOption; }
            set { SetProperty(ref _dbEditOption, value); }
        }
        public DatabaseEditViewModel(IEventAggregator ea, ISelectedService selectedService, ISettingsRepo settings)
        {
            _eventAggregator = ea;
            _selectedService = selectedService;
            _settingsRepo = settings;

            DbEditOption = new DbEditOption();

            _eventAggregator.GetEvent<SystemSelected>().Subscribe(PopulateExecutables);            

            PopulateExecutables("");

            ReplaceExeCommand = new DelegateCommand<string>(x =>
            {
                _eventAggregator.GetEvent<ReplaceExecutableEvent>().Publish(DbEditOption.ExecutableList.CurrentItem.ToString());
            });

            GetTableInfoCommand = new DelegateCommand<string>(x =>
            {
                _eventAggregator.GetEvent<GetTableInfoEvent>().Publish(x);
            });

            SetRadioGroup = new DelegateCommand<string>(x =>
            {
                var radioNumber = Convert.ToInt32(x);

                DbEditOption.RadioGroupSelected = radioNumber;
            });

            SetOptionsCommand = new DelegateCommand<string>(SetOptionsForTable);
        }

        private void SetOptionsForTable(string onOff)
        {
            Dictionary<string, bool> dict = new Dictionary<string, bool>();

            bool OnOrOff;

            if (onOff == "on") OnOrOff = true;
            else OnOrOff = false;

            switch (DbEditOption.RadioGroupSelected)
            {
                case 0:
                    dict.Add("Enabled", OnOrOff);
                    break;
                case 1:
                    dict.Add("HideDmd", OnOrOff);
                    break;
                case 2:
                    dict.Add("HideBackglass", OnOrOff);
                    break;
                default:
                    break;
            }

            _eventAggregator.GetEvent<SetExtraTableOptionsEvent>().Publish(dict);
            
        }

        private void PopulateExecutables(string obj)
        {
            var path = _selectedService.CurrentWorkingPath;

            GetExecutablesForSystem(path);
        }

        private void GetExecutablesForSystem(string emulatorPath)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(emulatorPath);

                var files = directoryInfo.GetFiles("*.exe");

                var exeList = new List<string>();

                for (int index = 0; index < files.Length; index++)
                {
                    var item = files[index];

                    exeList.Add(item.Name);
                }

                DbEditOption.ExecutableList = new ListCollectionView(exeList);

            }
            catch (Exception) { }
            
        }
    }
}
