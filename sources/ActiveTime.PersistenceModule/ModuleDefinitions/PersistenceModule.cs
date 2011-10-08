using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using DustInTheWind.ActiveTime.Persistence.Repositories;
using DustInTheWind.ActiveTime.Common;

namespace DustInTheWind.ActiveTime.RepositoryModule.ModuleDefinitions
{
    public class PersistenceModule : IModule
    {
        private IUnityContainer unityContainer;

        public PersistenceModule(IUnityContainer unityContainer)
        {
            if (unityContainer == null)
                throw new ArgumentNullException("unityContainer");

            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            //unityContainer.RegisterType<ITimeRecordRepository, TimeRecordRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<ITimeRecordRepository>(new TimeRecordRepository(), new ContainerControlledLifetimeManager());
        }
    }
}
