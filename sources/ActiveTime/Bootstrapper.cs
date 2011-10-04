using System;
using System.Windows;
using DustInTheWind.ActiveTime.UI.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override Microsoft.Practices.Prism.Modularity.IModuleCatalog CreateModuleCatalog()
        {
            //return base.CreateModuleCatalog();



            //ModuleCatalog moduleCatalog = new ModuleCatalog();
            //moduleCatalog.AddModule(typeof(ConfigurationModule))
            //    .AddModule(typeof(GatesModule))
            //    .AddModule(typeof(CatalogsModule), "ConfigurationModule")
            //    .AddModule(typeof(StatusInfoModule))
            //    .AddModule(typeof(ContentDetailsModule))
            //    .AddModule(typeof(MainMenuModule));

            //return moduleCatalog;



            //Uri uri = new Uri("/ModuleCatalog.xaml", UriKind.Relative);
            //Uri uri = new Uri("pack://application:,,,/ModuleCatalog.xaml", UriKind.Absolute);

            //Uri uri = new Uri("/ActiveTime;component/ModuleCatalog.xaml", UriKind.Relative);
            //ModuleCatalog moduleCatalog = Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(uri);
            
            //return moduleCatalog;


            var catalog = new DirectoryModuleCatalog();
            catalog.ModulePath = @".\Modules";
            return catalog;
        }

        protected override DependencyObject CreateShell()
        {
            MainWindow shell = Container.Resolve<MainWindow>();
            shell.Show();
            return shell;
        }
    }
}
