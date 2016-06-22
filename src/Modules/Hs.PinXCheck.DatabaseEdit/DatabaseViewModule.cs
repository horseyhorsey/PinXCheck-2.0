using Hs.PinXCheck.Base.Constants;
using Hs.PinXCheck.Base.PrismBase;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Hs.PinXCheck.Database.View
{
    public class DatabaseViewModule : PrismBaseModule
    {
        IRegionManager _regionManager;

        public DatabaseViewModule(IUnityContainer container, IRegionManager manager):base(container,manager)
        {
            _regionManager = manager;
        }

        public override void Initialize()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Views.DatabaseView));

            try
            {
                RegionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Views.MasterDatabaseView));
            }
            catch (Exception)
            {

                throw;
            }
            
        }

    }
}