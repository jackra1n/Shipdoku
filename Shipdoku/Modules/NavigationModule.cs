using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Shipdoku.Views;

namespace Shipdoku.Modules
{
    public class NavigationModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public NavigationModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(StartMenu));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StartMenu>();
            containerRegistry.RegisterForNavigation<Shipdoku.Views.Shipdoku>();
        }
    }
}
