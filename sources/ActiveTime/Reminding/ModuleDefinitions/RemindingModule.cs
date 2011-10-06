using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Reminding.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.Reminding.ModuleDefinitions
{
    class RemindingModule : IModule
    {
        private readonly IUnityContainer unityContainer;

        public RemindingModule(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IReminder, Reminder>(new ContainerControlledLifetimeManager());
        }
    }
}
