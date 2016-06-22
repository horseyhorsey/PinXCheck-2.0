using Hs.PinballX;
using Hs.PinXCheck.Base.Events;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hs.PinXCheck.Shell.ViewModels
{
    public class SystemSettingsFlyoutViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggreagator;

        #region Properties
        private PinballXSystem selectedSystem;
        public PinballXSystem SelectedSystem
        {
            get { return selectedSystem; }
            set { SetProperty(ref selectedSystem, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand SaveSystemToIniCommand { get; private set; }
        public DelegateCommand<string> SetFileCommand { get; private set; }
        public DelegateCommand<string> SetFolderCommand { get; private set; } 
        #endregion

        #region Services
        private ISystemsRepo _systemsRepo;
        private ISettingsRepo _settingsRepo;
        private IFileService _fileService;
        private IFolderService _folderService;
        #endregion

        public SystemSettingsFlyoutViewModel(IEventAggregator ea, ISystemsRepo systemsRepo,ISettingsRepo settings,
            IFileService fileService, IFolderService folderService)
        {
            _eventAggreagator = ea;
            _systemsRepo = systemsRepo;
            _settingsRepo = settings;
            _fileService = fileService;
            _folderService = folderService;

            _eventAggreagator.GetEvent<SystemSelected>().Subscribe(SystemChanged);

            SaveSystemToIniCommand = new DelegateCommand(SaveSystemToIni);

            SetFileCommand = new DelegateCommand<string>(SetFile);

            SetFolderCommand = new DelegateCommand<string>(SetFolder);
        }

        #region Methods

        private void SetFolder(string folderType)
        {
            try
            {
                _folderService.setFolderDialog();

                if (_folderService.SelectedFolder == null) return;

                var id = _systemsRepo.SystemsList.IndexOf(SelectedSystem);

                switch (folderType)
                {
                    case "TablePath":
                        _systemsRepo.SystemsList.ElementAt(id).TablePath = _folderService.SelectedFolder;
                        break;
                    case "NvramPath":
                        _systemsRepo.SystemsList.ElementAt(id).Nvrampath = _folderService.SelectedFolder;
                        break;
                    default:
                        break;
                }

                SystemChanged(_systemsRepo.SystemsList.ElementAt(id).Name);
            }
            catch (Exception) { }
            
        }

        private void SetFile(string PinballXFileType)
        {
            var fileAndPathArray = _fileService.GetFileNameDialog();

            if (!string.IsNullOrEmpty(fileAndPathArray[0]))
            {
                var id = _systemsRepo.SystemsList.IndexOf(SelectedSystem);

                switch (PinballXFileType)
                {
                    case "Executable":
                        _systemsRepo.SystemsList.ElementAt(id).WorkingPath = fileAndPathArray[0];
                        _systemsRepo.SystemsList.ElementAt(id).Executable = fileAndPathArray[1];
                        break;
                    case "BeforeFile":
                        _systemsRepo.SystemsList.ElementAt(id).LaunchBeforePath = fileAndPathArray[0];
                        _systemsRepo.SystemsList.ElementAt(id).LaunchBeforeexe = fileAndPathArray[1];
                        break;
                    case "AfterFile":
                        _systemsRepo.SystemsList.ElementAt(id).LaunchAfterWorkingPath = fileAndPathArray[0];
                        _systemsRepo.SystemsList.ElementAt(id).LaunchAfterexe = fileAndPathArray[1];
                        break;
                    default:
                        break;
                }

                SystemChanged(_systemsRepo.SystemsList.ElementAt(id).Name);
            }

        }

        private void SaveSystemToIni()
        {
            var pinballXConfig = _settingsRepo.PinXCheckSettings.PinballXPath + "\\Config\\PinballX.ini";

            if (File.Exists(pinballXConfig))
            {
                try
                {
                    _systemsRepo.SaveSystemToIni(pinballXConfig, SelectedSystem);
                }
                catch (Exception e) { }
            }
            
        }

        private void SystemChanged(string systemName)
        {
            try
            {
                var system = _systemsRepo.SystemsList.Where(x => x.Name == systemName);
                SelectedSystem = new PinballXSystem();
                SelectedSystem = system.FirstOrDefault();
            }
            catch (Exception e) { }
        }

        #endregion
    }
}
