using System.Reflection;
using Autofac;
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Application.UseCases.StartRecording;
using DustInTheWind.ActiveTime.Common.Infrastructure;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Presentation;
using DustInTheWind.ActiveTime.Common.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Logging;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Module;
using DustInTheWind.ActiveTime.Presentation.Services;
using DustInTheWind.ActiveTime.Presentation.ViewModels;
using DustInTheWind.ActiveTime.Presentation.Views;
using DustInTheWind.ActiveTime.Recording.Module.Services;
using DustInTheWind.ActiveTime.TrayGui.Module;
using DustInTheWind.ActiveTime.TrayGui.ViewModels;
using DustInTheWind.ActiveTime.TrayGui.Views;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace DustInTheWind.ActiveTime
{
    internal class Bootstrapper2
    {
        private readonly IContainer container;

        public Bootstrapper2()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            ConfigureServices(containerBuilder);
            container = containerBuilder.Build();
        }

        private static void ConfigureServices(ContainerBuilder containerBuilder)
        {
            // GUI - Tray Icon
            containerBuilder.RegisterType<TrayIconModule>().AsSelf();
            containerBuilder.RegisterType<TrayIconView>().AsSelf();
            containerBuilder.RegisterType<TrayIconPresenter>().AsSelf();

            // GUI - Windows
            containerBuilder.RegisterType<MainWindow>().AsSelf();
            containerBuilder.RegisterType<MainViewModel>().AsSelf();
            containerBuilder.RegisterType<MainMenuViewModel>().AsSelf();
            containerBuilder.RegisterType<StatusInfoViewModel>().AsSelf();
            containerBuilder.RegisterType<AboutWindow>().AsSelf();
            containerBuilder.RegisterType<AboutViewModel>().AsSelf();

            // Register services.
            containerBuilder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            containerBuilder.RegisterType<CurrentDay>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<ScribeEx>().AsSelf();
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            containerBuilder.RegisterType<EventBus>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<InMemoryState>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<ScheduledJobs>().AsSelf().SingleInstance();

            // Register WPF related services.
            containerBuilder.RegisterType<AutofacWindowFactory>().As<IWindowFactory>();
            containerBuilder.RegisterType<ApplicationService>().As<IApplicationService>().SingleInstance();
            containerBuilder.RegisterType<ShellNavigator>().As<IShellNavigator>().SingleInstance();
            containerBuilder.RegisterType<DispatcherService>().AsSelf().SingleInstance();

            // Register business services.
            containerBuilder.RegisterType<SystemClock>().As<ISystemClock>().SingleInstance();
            containerBuilder.RegisterType<StatusInfoService>().As<IStatusInfoService>().SingleInstance();
            containerBuilder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            containerBuilder.RegisterType<AutofacUnitOfWorkFactory>().As<IUnitOfWorkFactory>().SingleInstance();
            containerBuilder.RegisterType<Dwarfs>().AsSelf().SingleInstance();

            containerBuilder.RegisterType<RecorderTimer>().SingleInstance();

            Assembly useCasesAssembly = typeof(StartRecordingRequest).Assembly;
            containerBuilder.RegisterMediatR(useCasesAssembly);
        }

        public void Run()
        {
            RegisterGuiShells();
            InitializeModules();

            ScheduledJobs scheduledJobs = container.Resolve<ScheduledJobs>();
            RecorderJob recorderJob = container.Resolve<RecorderJob>();
            scheduledJobs.Add(recorderJob);


            IApplicationService applicationService = container.Resolve<IApplicationService>();
            applicationService.Start();
        }

        private void RegisterGuiShells()
        {
            IShellNavigator shellNavigator = container.Resolve<IShellNavigator>();

            // Register shells in the shell navigator. (Needed for shell navigation.)
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.MainShell, typeof(MainWindow)));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.MessageShell, typeof(MessageWindow), ShellNames.MainShell));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.AboutShell, typeof(AboutWindow), ShellNames.MainShell));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.OverviewShell, typeof(OverviewWindow), ShellNames.MainShell));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.CalendarShell, typeof(CalendarWindow), ShellNames.MainShell));
        }

        private void InitializeModules()
        {
            TrayIconModule trayIconModule = container.Resolve<TrayIconModule>();
            trayIconModule.Initialize();
        }
    }
}