using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using Shipdoku.Interfaces;
using Shipdoku.Modules;
using Shipdoku.Services;
using Shipdoku.Views;
using System.Windows;

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
            moduleCatalog.AddModule<NavigationModule>();
        }
    } 
} 