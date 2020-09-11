using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using ShipdokuGUI.Views;

namespace ShipdokuGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // ToDo: register needed services here
            // containerRegistry.Register<ICustomerStore, DbCustomerStore>();
        }

        protected override Window CreateShell()
        {
            var w = Container.Resolve<Shell>();
            return w;
        }
    }
}
