using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Main.Services;
using DustInTheWind.ActiveTime.Main.Views;
using DustInTheWind.ActiveTime.Persistence.Repositories;
using DustInTheWind.ActiveTime.Reminding.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

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
            unityContainer.RegisterType<IStatusInfoService, StatusInfoService>(new ContainerControlledLifetimeManager());

            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(MainView));
            regionManager.RegisterViewWithRegion(RegionNames.StatusInfoRegion, typeof(StatusInfoView));
        }
    }
}
