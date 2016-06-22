using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Services;
using Hs.PinXCheck.Settings;
using Hs.PinXCheck.SystemViewer.Views;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace Hs.PinXCheck.SystemViewer
{
    public class SystemsModule : PrismBaseModule
    {

        IRegionManager _regionManager;

        public SystemsModule(IUnityContainer container, IRegionManager manager) : base(container, manager)
        {
            _regionManager = manager;
            
        }

        public override void Initialize()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.SystemsRegion, typeof(SystemsView));
        }
    }
}