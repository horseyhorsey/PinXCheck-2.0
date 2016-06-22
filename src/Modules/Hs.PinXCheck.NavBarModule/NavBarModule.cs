using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Base.Services;
using Hs.PinXCheck.SystemViewer.Views;
using Microsoft.Practices.Unity;
using NavBarModule.Views;
using Prism.Regions;

namespace Hs.PinXCheck.NavBarModule
{
    public class NavBarModule : PrismBaseModule
    {

        IRegionManager _regionManager;

        public NavBarModule(IUnityContainer container, IRegionManager manager):base(container,manager)
        {
            _regionManager = manager;
        }

        public override void Initialize()
        {
                   
            RegionManager.RegisterViewWithRegion(RegionNames.NavBarRegion, typeof(NavBarView));

            RegionManager.RegisterViewWithRegion(RegionNames.SystemsRegion, typeof(SystemsView));            

        }
        
    }
}