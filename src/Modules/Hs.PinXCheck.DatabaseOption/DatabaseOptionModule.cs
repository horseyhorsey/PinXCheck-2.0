using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.PrismBase;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace Hs.PinXCheck.Database.Pane
{
    public class DatabaseOptionModule : PrismBaseModule
    {

        IRegionManager _regionManager;

        public DatabaseOptionModule(IUnityContainer container, IRegionManager manager) : base(container, manager)
        {
            _regionManager = manager;
        }

        public override void Initialize()
        {
            //UnityContainer.RegisterType<ISettingsRepo, SettingsRepo>(new ContainerControlledLifetimeManager());
            RegionManager.RegisterViewWithRegion(RegionNames.LeftPaneRegion,typeof(Views.DatabaseOptionView));
        }
    }
}