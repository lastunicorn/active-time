using System;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.RecorderModule.ModuleDefinitions
{
    public class RecorderModule : IModule
    {
        private IUnityContainer unityContainer;

        public RecorderModule(IUnityContainer unityContainer)
        {
            if (unityContainer == null)
                throw new ArgumentNullException("unityContainer");

            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IRecorder, Recorder>(new ContainerControlledLifetimeManager());
        }
    }
}
