using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.Interfaces;
using MahApps.Metro.Controls;
using Prism.Commands;
using Prism.Regions;
using System.Linq;
using System.Windows.Input;
using System;
using MahApps.Metro.Controls.Dialogs;

namespace Hs.PinXCheck.Base.Services
{
    public class DialogService : IDialogService
    {
        IRegionManager _regionManager;

        public ICommand ShowDialogCommand { get; private set; }

        public DialogService(IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;

            ShowDialogCommand = new DelegateCommand<string>(ShowDialog, CanShowDialog);
            applicationCommands.ShowFlyoutCommand.RegisterCommand(ShowDialogCommand);
        }

        public void ShowDialog(string dialogName)
        {
            var region = _regionManager.Regions[RegionNames.FlyoutRegion];

            if (region != null)
            {
                var dialog = region.Views.Where(view => view is IDialogView && ((IDialogView)view)
                .DialogName.Equals(dialogName))
                .FirstOrDefault() as ProgressDialogController;

                if (dialog != null)
                {
                    
                }
            }
        }

        public bool CanShowDialog(string dialogName)
        {
            return true;
        }

    }
}
