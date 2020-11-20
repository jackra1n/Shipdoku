using Prism.Commands;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Shipdoku.Views;
using System;
using Unity;

namespace Shipdoku.Modules
{
    public class NavigationModule : IModule
    {
        IRegionManager _regionManager;

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
