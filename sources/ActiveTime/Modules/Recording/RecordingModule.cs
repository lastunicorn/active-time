using System;
using System.Collections.Generic;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.MainModule.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.Recording.ModuleDefinitions
{
    public class RecordingModule : IModule
    {
        private IUnityContainer unityContainer;

        public RecordingModule(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IRecorder, Recorder>(new ContainerControlledLifetimeManager());
        }
    }
}
