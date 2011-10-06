using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Main.Services;
using DustInTheWind.ActiveTime.Main.Views;

namespace DustInTheWind.ActiveTime.StatusInfo.ModuleDefinitions
{
    public class StatusInfoModule : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IRegionManager regionManager;

        public StatusInfoModule(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IStatusInfoService, StatusInfoService>(new ContainerControlledLifetimeManager());

            regionManager.RegisterViewWithRegion(RegionNames.StatusInfoRegion, typeof(StatusInfoView));
        }
    }
}
