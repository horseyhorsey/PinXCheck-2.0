using Hs.PinXCheck.Base.Interfaces;
using Hs.PinXCheck.Base.PrismBase;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Hs.PinXCheck.Settings
{
    public class SettingsModule : PrismBaseModule
    {
        IRegionManager _regionManager;

        public SettingsModule(IUnityContainer container, IRegionManager manager):base(container,manager)
        {
            _regionManager = manager;
        }

        public override void Initialize()
        {

        }
    }
}