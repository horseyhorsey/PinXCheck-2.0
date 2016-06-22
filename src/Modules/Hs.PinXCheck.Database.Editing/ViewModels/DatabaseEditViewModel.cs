using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
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
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseEditViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private ISelectedService _selectedService;
        private ISettingsRepo _settingsRepo;

        private ICollectionView executableList;
        public ICollectionView ExecutableList
        {
            get { return executableList; }
            set { SetProperty(ref executableList, value); }
        }

        public DelegateCommand<string> ReplaceExeCommand { get; private set; }
        public DelegateCommand<string> GetTableInfoCommand { get; private set; }
        public DelegateCommand<string> SetRadioGroup { get; private set; }
        public DelegateCommand<string> SetOptionsCommand { get; private set; } 


        private int radioGroupSelected = 0;
        public int RadioGroupSelected
        {
            get { return radioGroupSelected; }
            set { SetProperty(ref radioGroupSelected, value); }
        }


        public DatabaseEditViewModel(IEventAggregator ea, ISelectedService selectedService, ISettingsRepo settings)
        {
            _eventAggregator = ea;
            _selectedService = selectedService;
            _settingsRepo = settings;

            _eventAggregator.GetEvent<SystemSelected>().Subscribe(PopulateExecutables);            

            PopulateExecutables("");

            ReplaceExeCommand = new DelegateCommand<string>(x =>
            {
                _eventAggregator.GetEvent<ReplaceExecutableEvent>().Publish(ExecutableList.CurrentItem.ToString());
            });

            GetTableInfoCommand = new DelegateCommand<string>(x =>
            {
                _eventAggregator.GetEvent<GetTableInfoEvent>().Publish(x);
            });

            SetRadioGroup = new DelegateCommand<string>(x =>
            {
                var radioNumber = Convert.ToInt32(x);

                RadioGroupSelected = radioNumber;
            });

            SetOptionsCommand = new DelegateCommand<string>(SetOptionsForTable);
        }

        private void SetOptionsForTable(string onOff)
        {
            Dictionary<string, bool> dict = new Dictionary<string, bool>();

            bool OnOrOff;

            if (onOff == "on") OnOrOff = true;
            else OnOrOff = false;

            switch (RadioGroupSelected)
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

                ExecutableList = new ListCollectionView(exeList);

            }
            catch (Exception) { }
            
        }
    }
}
