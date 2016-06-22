using Prism.Commands;
using System;
using Hs.PinXCheck.Base.PrismBase;
using System.Collections.ObjectModel;
using Hs.PinXCheck.Shell.Models;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.Services;
using Prism.Events;
using Hs.PinXCheck.Base.Events;

namespace Hs.PinXCheck.Shell.ViewModels
{
    public class SettingsFlyoutViewModel : ViewModelBase
    {

        #region Properties
        public Setting.Settings PinXCheckSettings
        {
            get { return _pinXCheckSettingsRepo.PinXCheckSettings; }
            set { _pinXCheckSettingsRepo.PinXCheckSettings = value; }
        }
        #endregion

        #region Theme Properties
        public ObservableCollection<string> GuiThemes { get; set; }
        private string _currentThemeColor;
        public string CurrentThemeColor
        {
            get { return _currentThemeColor; }
            set
            {
                SetProperty(ref _currentThemeColor, value);
                changeGuiTheme();
            }
        }

        private bool _isDarkTheme;        
        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set
            {
                SetProperty(ref _isDarkTheme, value);
                changeGuiTheme();
            }
        }

        private bool isMaximized;
        public bool IsMaximized
        {
            get { return isMaximized; }
            set { SetProperty(ref isMaximized, value); }
        }
        #endregion

        #region Services
        private ISettingsRepo _pinXCheckSettingsRepo;
        private IFolderService _folderService;
        private IEventAggregator _eventAggreagator;
        #endregion

        #region Commands
        public DelegateCommand SaveSettingsCommand { get; private set; }
        public DelegateCommand<string> SetFolderCommand { get; private set; }
        #endregion

        #region Contructor
        public SettingsFlyoutViewModel(ISettingsRepo settings, IFolderService folderService, IEventAggregator ea)
        {
            _pinXCheckSettingsRepo = settings;
            _folderService = folderService;

            _eventAggreagator = ea;

            try { _pinXCheckSettingsRepo.LoadPinXCheckSettings(); }
            catch (Exception e) { }

            SetApplicationSettings();

            //Setup themes for combobox binding            
            GuiThemes = new ObservableCollection<string>(MahAppTheme.AvailableThemes);

            SaveSettingsCommand = new DelegateCommand(SaveSettings);
            SetFolderCommand = new DelegateCommand<string>(SetFolder);
            
        }

        #endregion

        #region Methods

        private void SetApplicationSettings()
        {
            CurrentThemeColor = Properties.Settings.Default.GuiColor;
            IsDarkTheme = Properties.Settings.Default.GuiTheme;
            IsMaximized = Properties.Settings.Default.IsMaximized;

            if (IsMaximized)
                Hs.PinXCheck.Shell.App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
        }

        private void SetFolder(string property)
        {            
            var userPath = "";

            _folderService.setFolderDialog();

            if (!string.IsNullOrEmpty(_folderService.SelectedFolder))
            {
                userPath = _folderService.SelectedFolder;

                UpdatePath(property, userPath);
            }
        }

        private void UpdatePath(string property, string userPath)
        {
            switch (property)
            {
                case "PinballXPath":
                    PinXCheckSettings.PinballXPath = userPath;                    
                    break;
                default:
                    break;
            };
        }

        private void changeGuiTheme()
        {
            string darkOrLight = string.Empty;
            if (IsDarkTheme)
                darkOrLight = "BaseDark";
            else
                darkOrLight = "BaseLight";

            // now set the theme
            try
            {
                MahApps.Metro.ThemeManager.ChangeAppStyle(System.Windows.Application.Current,
                            MahApps.Metro.ThemeManager.GetAccent(CurrentThemeColor),
                            MahApps.Metro.ThemeManager.GetAppTheme(darkOrLight));
            }
            catch (System.Exception) { }

        }

        public void SaveSettings()
        {
            Properties.Settings.Default.GuiColor = CurrentThemeColor;
            Properties.Settings.Default.GuiTheme = IsDarkTheme;
            Properties.Settings.Default.IsMaximized = IsMaximized;

            Properties.Settings.Default.Save();

            _pinXCheckSettingsRepo.PinXCheckSettings = PinXCheckSettings;

            _pinXCheckSettingsRepo.SavePinXCheckSettings();
        }
        #endregion

    }
}
