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
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Hs.PinXCheck.SystemViewer.ViewModels
{
    public class SystemsViewModel : ViewModelBase
    {
        #region Properties
        private ImageSource systemLogo;

        private ICollectionView pinballXSystems ;
        public ICollectionView PinballXSystems
        {
            get { return pinballXSystems; }
            set { SetProperty(ref pinballXSystems, value); }
        }

        private bool systemComboBoxEnabled = true;
        public bool SystemComboBoxEnabled
        {
            get { return systemComboBoxEnabled; }
            set { SetProperty(ref systemComboBoxEnabled, value); }
        }

        #region Services
        private ISelectedService _selectedService;
        private ISystemsRepo _systemRepo;
        private ISettingsRepo _settingsRepo;
        private IEventAggregator _eventAggregator;
        #endregion

        public ImageSource SystemLogo
        {
            get { return systemLogo; }
            set { SetProperty(ref systemLogo, value); }
        }
        #endregion

        #region CTORs
        public SystemsViewModel(ISelectedService selected, ISettingsRepo settings, ISystemsRepo systems
            , IEventAggregator ea
            )
        {
            _selectedService = selected;
            _systemRepo = systems;
            _settingsRepo = settings;
            _eventAggregator = ea;    

            _systemRepo.BuildSystemFromIni(_settingsRepo.PinXCheckSettings.PinballXPath + "\\Config\\PinballX.ini");

            PinballXSystems = new ListCollectionView
                (_systemRepo.SystemsList);            

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                _selectedService.SystemLogo = SelectedService.SetBitmapFromUri(new Uri(baseDirectory + @"\Images\logo.png"));

                SystemLogo = _selectedService.SystemLogo;
            }
            catch (Exception) { }

            _eventAggregator.GetEvent<DisableControlsEvent>().Subscribe(b =>
            {
                SystemComboBoxEnabled = b;
            });

            PinballXSystems_CurrentChanged(null, null);

            PinballXSystems.CurrentChanged += PinballXSystems_CurrentChanged;
            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Update the system settings from a pinballx config
        /// Set the tables path, system type and publish an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PinballXSystems_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                var currentSystemName = PinballXSystems.CurrentItem as PinballX.PinballXSystem;

                _selectedService.CurrentSystem = currentSystemName.Name;

                _systemRepo.UpdateFromIni(_settingsRepo.PinXCheckSettings.PinballXPath + "\\Config\\PinballX.ini",
                    currentSystemName.Id);

                _selectedService.CurrentTablePath = currentSystemName.TablePath;
                _selectedService.CurrentWorkingPath = currentSystemName.WorkingPath;
                _selectedService.CurrentSystemType = currentSystemName.PinXType;
                _selectedService.CurrentSystemDefaultExecutable = currentSystemName.Executable;

                _eventAggregator.GetEvent<SystemSelected>().Publish(currentSystemName.Name);
            }
            catch (Exception) { }
            
        }
        #endregion
    }
}
