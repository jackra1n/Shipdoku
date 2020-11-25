using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
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
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IShipdokuGenerator, ShipdokuGenerator>();
            containerRegistry.Register<IExportService, ExportService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Shipdoku.Modules.NavigationModule>();
        }
    } 
} 