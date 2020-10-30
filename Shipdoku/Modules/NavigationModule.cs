using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Text;
using Shipdoku.ViewModels;

namespace Shipdoku.Modules
{
    public class NavigationModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StartMenuViewModel>();
        }
    }
}
