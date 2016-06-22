using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.PrismBase;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace Hs.PinXCheck.DescriptionMatch
{
    public class DescriptionMatchModule : PrismBaseModule
    {

        IRegionManager _regionManager;

        public DescriptionMatchModule(IUnityContainer container, IRegionManager manager) : base(container, manager)
        {
            _regionManager = manager;
        }

        public override void Initialize()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.DbOptionsMatchTab, typeof(Views.DescriptionMatchView));
        }
    }
}