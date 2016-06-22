using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.PrismBase;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace Hs.PinXCheck.Database.Editing
{
    public class DatabaseEditModule : PrismBaseModule
    {

        IRegionManager _regionManager;

        public DatabaseEditModule(IUnityContainer container, IRegionManager manager) : base(container, manager)
        {
            _regionManager = manager;
        }

        public override void Initialize()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.DatabasesRegion, typeof(Views.SelectDatabaseView));

            RegionManager.RegisterViewWithRegion(RegionNames.DbOptionsEditTab, typeof(Views.DatabaseOptions));

            RegionManager.RegisterViewWithRegion(RegionNames.DbOptionsEditTab, typeof(Views.DatabaseEditView));

            RegionManager.RegisterViewWithRegion(RegionNames.DbOptionsEditTab, typeof(Views.GameFilterView));
        }
    }
}