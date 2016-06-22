using Microsoft.Practices.Unity;
using Prism.Unity;
using Hs.PinXCheck.Shell.Views;
using System.Windows;
using Prism.Regions;
using Prism.Modularity;
using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base;
using Hs.PinXCheck.Base.Services;
using Hs.PinXCheck.SystemViewer.Views;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Settings;
using Hs.PinXCheck.Services;

namespace Hs.PinXCheck.Shell
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            // Register view
            var regionManager = this.Container.Resolve<IRegionManager>();
            if (regionManager != null)
            {
                regionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(SettingsFlyout));
                regionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(SystemSettingsFlyout));                
            }

            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            //base.ConfigureModuleCatalog();
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;

            moduleCatalog.AddModule(typeof(SettingsModule));
            moduleCatalog.AddModule(typeof(ServiceModule));
            moduleCatalog.AddModule(typeof(UnusedTables.UnusedTablesModule));
            moduleCatalog.AddModule(typeof(Database.View.DatabaseViewModule));
            moduleCatalog.AddModule(typeof(Database.Pane.DatabaseOptionModule));
            moduleCatalog.AddModule(typeof(SystemViewer.SystemsModule));                  
            moduleCatalog.AddModule(typeof(DescriptionMatch.DescriptionMatchModule));
            moduleCatalog.AddModule(typeof(Database.Editing.DatabaseEditModule));
            moduleCatalog.AddModule(typeof(Media.Pane.PaneModule));
            
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IApplicationCommands, ApplicationCommandsProxy>();
            Container.RegisterInstance<IFlyoutService>(Container.Resolve<FlyoutService>());
            Container.RegisterInstance<IDialogService>(Container.Resolve<DialogService>());

            Container.RegisterInstance<ISelectedService>(Container.Resolve<SelectedService>());
            Container.RegisterInstance<IFolderService>(Container.Resolve<FolderService>());
            //Container.RegisterType<ISystemsRepo, SystemsRepo>(); 
            //Container.RegisterInstance<ISettingsRepo>(Container.Resolve<SettingsRepo>());
            Container.RegisterInstance<ISettingsRepo>(Container.Resolve<SettingsRepo>());
            Container.RegisterInstance<ISystemsRepo>(Container.Resolve<SystemsRepo>());
            Container.RegisterInstance<ITablesRepo>(Container.Resolve<TablesRepo>());
            Container.RegisterInstance<IFileService>(Container.Resolve<FileService>());

        }
    }
}
