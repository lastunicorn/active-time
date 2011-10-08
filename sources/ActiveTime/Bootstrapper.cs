using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System;
using DustInTheWind.ActiveTime.Common;

namespace DustInTheWind.ActiveTime
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }

        protected override Microsoft.Practices.Prism.Modularity.IModuleCatalog CreateModuleCatalog()
        {
            Uri uri = new Uri("/ActiveTime;component/ModuleCatalog.xaml", UriKind.Relative);
            ModuleCatalog moduleCatalog = Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(uri);

            return moduleCatalog;


            //var catalog = new DirectoryModuleCatalog();
            //catalog.ModulePath = @".\Modules";
            //return catalog;
        }

        protected override DependencyObject CreateShell()
        {
            return null;
        }
    }
}
