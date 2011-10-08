using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Reminding.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.ReminderModule.ModuleDefinitions
{
    public class ReminderModule : IModule
    {
        private readonly IUnityContainer unityContainer;

        public ReminderModule(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IReminder, Reminder>(new ContainerControlledLifetimeManager());
        }
    }
}
