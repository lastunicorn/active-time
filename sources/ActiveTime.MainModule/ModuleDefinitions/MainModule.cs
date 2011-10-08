using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.MainModule.Services;
using DustInTheWind.ActiveTime.MainModule.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.MainModule.ModuleDefinitions
{
    public class MainModule : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IRegionManager regionManager;

        private IMainService mainService;

        public MainModule(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IApplicationService, ApplicationService>();

            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(MainView));
        }
    }
}
