using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.Events;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using Prism.Regions;
using System.Windows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hs.PinXCheck.Shell.Views
{

    public partial class MainWindow : MetroWindow
    {
        private IEventAggregator _eventAggregator;
        private ProgressDialogController controller;

        public MainWindow(IRegionManager regionManager, IEventAggregator ea)
        {
            InitializeComponent();

            _eventAggregator = ea;

            if (regionManager != null)
                SetRegionManager(regionManager, flyoutsControlRegion, RegionNames.FlyoutRegion);

            _eventAggregator.GetEvent<ShowDialogEvent>().Subscribe(OpenDialog);

        }

        void SetRegionManager(IRegionManager regionManager, DependencyObject regionTarget, string regionName)
        {
            RegionManager.SetRegionName(regionTarget, regionName);
            RegionManager.SetRegionManager(regionTarget, regionManager);
        }

        async void OpenDialog(string message)
        {
            var msg = message.Split(',');
            var settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Ok";
            controller = await this.ShowProgressAsync(msg[0], msg[1], true, settings);
            controller.Maximum = 100;
            controller.Minimum = 0;

            Thread.Sleep(1000);
            controller.SetIndeterminate();
            controller.Canceled += Controller_Canceled;            

            await Task.Run(() => runProgress());            

        }

        private async void runProgress()
        {

            controller.SetTitle("Complete");
            controller.SetMessage("Database saved");
            controller.SetIndeterminate();
            
            await controller.CloseAsync();


        }

        private void Controller_Canceled(object sender, EventArgs e)
        {
            controller.CloseAsync();
        }

        void CloseDialog(string obj)
        {
             
        }

    }
}
