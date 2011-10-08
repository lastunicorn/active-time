using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Main.Services;
using DustInTheWind.ActiveTime.Persistence.Repositories;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.Recording.ModuleDefinitions
{
    public class RecordingModule : IModule
    {
        private readonly IUnityContainer unityContainer;

        public RecordingModule(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            unityContainer.RegisterInstance<ITimeRecordRepository>(new TimeRecordRepository(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IRecorder, Recorder>(new ContainerControlledLifetimeManager());

            IRecorder recorder = unityContainer.Resolve<IRecorder>();
        }
    }
}
