using System.Windows;
using DustInTheWind.ActiveTime.Main.ModuleDefinitions;
using DustInTheWind.ActiveTime.Reminding.ModuleDefinitions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using DustInTheWind.ActiveTime.Recording.ModuleDefinitions;
using DustInTheWind.ActiveTime.StatusInfo.ModuleDefinitions;

namespace DustInTheWind.ActiveTime
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override Microsoft.Practices.Prism.Modularity.IModuleCatalog CreateModuleCatalog()
        {
            //return base.CreateModuleCatalog();



            ModuleCatalog moduleCatalog = new ModuleCatalog();
            moduleCatalog.AddModule(typeof(MainModule), "RecordingModule", "StatusInfoModule")
                .AddModule(typeof(RecordingModule), "RemindingModule")
                .AddModule(typeof(RemindingModule))
                .AddModule(typeof(StatusInfoModule));
            
            return moduleCatalog;



            //Uri uri = new Uri("/ModuleCatalog.xaml", UriKind.Relative);
            //Uri uri = new Uri("pack://application:,,,/ModuleCatalog.xaml", UriKind.Absolute);

            //Uri uri = new Uri("/ActiveTime;component/ModuleCatalog.xaml", UriKind.Relative);
            //ModuleCatalog moduleCatalog = Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(uri);

            //return moduleCatalog;


            //var catalog = new DirectoryModuleCatalog();
            //catalog.ModulePath = @".\Modules";
            //return catalog;
        }

        protected override DependencyObject CreateShell()
        {
            Shell shell = Container.Resolve<Shell>();
            shell.Show();
            return shell;
        }
    }
}
