using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Shipdoku.Views;
using System;

namespace Shipdoku.Modules
{
    public class NavigationModule : IModule
    {
        IRegionManager _regionManager;

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StartMenu>();
            containerRegistry.RegisterForNavigation<Shipdoku.Views.Shipdoku>();
        }
        public void Initialize()
        {
            _regionManager.RequestNavigate("ContentRegion", new Uri("Shipdoku", UriKind.Relative));
        }
    }
}
