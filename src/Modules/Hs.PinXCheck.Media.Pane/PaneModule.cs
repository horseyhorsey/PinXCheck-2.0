using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.PrismBase;
using Hs.PinXCheck.Media.Pane.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Hs.PinXCheck.Media.Pane
{
    public class PaneModule : PrismBaseModule
    {
        IRegionManager _regionManager;

        public PaneModule(IUnityContainer container, IRegionManager manager) : base(container, manager)
        {
            _regionManager = manager;
        }

        public override void Initialize()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.RightPaneRegion,typeof(MediaPaneView));
        }
    }
}