using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.UnusedTables.Views;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace Hs.PinXCheck.UnusedTables
{
    public class UnusedTablesModule : PrismBaseModule
    {
        IRegionManager _regionManager;

        public UnusedTablesModule(IUnityContainer container, IRegionManager manager) : base(container, manager)
        {
            _regionManager = manager;
        }

        public override void Initialize()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.DbOptionsTableTab, typeof(UnusedTablesView));
        }
    }
}