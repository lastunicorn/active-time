using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Main.Services;
using DustInTheWind.ActiveTime.Main.Views;
using DustInTheWind.ActiveTime.Persistence.Repositories;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.Main.ModuleDefinitions
{
    public class MainModule : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IRegionManager regionManager;

        public MainModule(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IRecorder, Recorder>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IReminder, Reminder>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ITimeRecordRepository, TimeRecordRepository>(new ContainerControlledLifetimeManager());
            //unityContainer.RegisterType<ITimeRecordRepository, ExportersManager>(new ContainerControlledLifetimeManager());

            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof (MainView));
        }
    }
}
