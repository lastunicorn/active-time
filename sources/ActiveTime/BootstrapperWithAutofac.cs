using System.Collections.Generic;
using System.Reflection;
using System.Windows.Documents;
using Autofac;
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Application.UseCases.StartRecording;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Jobs;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Presentation;
using DustInTheWind.ActiveTime.Common.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.System;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Logging;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Module;
using DustInTheWind.ActiveTime.Presentation.Services;
using DustInTheWind.ActiveTime.Presentation.ViewModels;
using DustInTheWind.ActiveTime.Presentation.Views;
using DustInTheWind.ActiveTime.Recording.Module.Services;
using DustInTheWind.ActiveTime.TrayGui.Module;
using DustInTheWind.ActiveTime.TrayGui.ViewModels;
using DustInTheWind.ActiveTime.TrayGui.Views;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace DustInTheWind.ActiveTime
{
    internal class BootstrapperWithAutofac
    {
        private readonly IContainer container;

        public BootstrapperWithAutofac()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            ConfigureServices(containerBuilder);
            container = containerBuilder.Build();
        }

        private static void ConfigureServices(ContainerBuilder containerBuilder)
        {
            // Database
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // GUI - Tray Icon
            containerBuilder.RegisterType<TrayIconModule>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<TrayIconView>().AsSelf();
            containerBuilder.RegisterType<TrayIconPresenter>().AsSelf();

            // GUI - Windows
            containerBuilder.RegisterType<MainWindow>().AsSelf();
            containerBuilder.RegisterType<MainViewModel>().AsSelf();
            containerBuilder.RegisterType<MainMenuViewModel>().AsSelf();
            containerBuilder.RegisterType<StatusInfoViewModel>().AsSelf();
            containerBuilder.RegisterType<FrontViewModel>().AsSelf();
            containerBuilder.RegisterType<CurrentDateViewModel>().AsSelf();
            containerBuilder.RegisterType<TimeReportViewModel>().AsSelf();
            containerBuilder.RegisterType<CommentsViewModel>().AsSelf();
            containerBuilder.RegisterType<DayRecordsViewModel>().AsSelf();

            containerBuilder.RegisterType<CalendarWindow>().AsSelf();
            containerBuilder.RegisterType<CalendarViewModel>().AsSelf();

            containerBuilder.RegisterType<OverviewWindow>().AsSelf();
            containerBuilder.RegisterType<OverviewViewModel>().AsSelf();

            containerBuilder.RegisterType<AboutWindow>().AsSelf();
            containerBuilder.RegisterType<AboutViewModel>().AsSelf();

            // GUI - Services
            containerBuilder.RegisterType<AutofacWindowFactory>().As<IWindowFactory>();
            containerBuilder.RegisterType<ApplicationService>().As<IApplicationService>().SingleInstance();
            containerBuilder.RegisterType<ShellNavigator>().As<IShellNavigator>().SingleInstance();
            containerBuilder.RegisterType<DispatcherService>().AsSelf().SingleInstance();

            // Register singleton services.
            containerBuilder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            containerBuilder.RegisterType<CurrentDay>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<EventBus>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<InMemoryState>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<StatusInfoService>().As<IStatusInfoService>().SingleInstance();
            containerBuilder.RegisterType<CurrentDay>().AsSelf().SingleInstance();

            // Register services.
            containerBuilder.RegisterType<SystemClock>().As<ISystemClock>();
            containerBuilder.RegisterType<ScribeEx>().AsSelf();
            containerBuilder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            containerBuilder.RegisterType<AutofacUnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            containerBuilder.RegisterType<Dwarfs>().AsSelf().SingleInstance();

            // Jobs
            containerBuilder.RegisterType<ScheduledJobs>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<RecorderJob>();

            // MediatR
            Assembly useCasesAssembly = typeof(StartRecordingRequest).Assembly;
            containerBuilder.RegisterMediatR(useCasesAssembly);

            //containerBuilder.Register<ServiceFactory>(outerContext =>
            //{
            //    var innerContext = outerContext.Resolve<IComponentContext>();

            //    return serviceType => innerContext.Resolve(serviceType);
            //});
        }

        public void Run()
        {
            RegisterJobs();
            RegisterGuiShells();

            InitializeModules();

            IApplicationService applicationService = container.Resolve<IApplicationService>();
            applicationService.Start();
        }

        private void RegisterJobs()
        {
            ScheduledJobs scheduledJobs = container.Resolve<ScheduledJobs>();
            RecorderJob recorderJob = container.Resolve<RecorderJob>();
            scheduledJobs.Add(recorderJob);
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