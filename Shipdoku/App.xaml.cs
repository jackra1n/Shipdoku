using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using Shipdoku.Interfaces;
using Shipdoku.Services;
using Shipdoku.Views;

namespace Shipdoku
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IShipdokuGenerator, ShipdokuGenerator>();
        }

        protected override Window CreateShell()
        {
            var w = Container.Resolve<Shell>();
            return w;
        }
    }
}
