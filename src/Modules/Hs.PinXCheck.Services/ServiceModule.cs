using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Settings;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace Hs.PinXCheck.Services
{
    public class ServiceModule : PrismBaseModule
    {

        IRegionManager _regionManager;

        public ServiceModule(IUnityContainer container, IRegionManager manager) : base(container, manager)
        {
            _regionManager = manager;
        }

        public override void Initialize()
        {
            
        }
    }
}