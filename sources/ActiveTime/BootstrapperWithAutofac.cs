using System.Collections.Generic;
using System.Reflection;
using System.Windows.Documents;
using Autofac;
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Application.Recording.StartRecording;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Presentation;
using DustInTheWind.ActiveTime.Common.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.System;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Jobs;
using DustInTheWind.ActiveTime.Logging;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Module;
using DustInTheWind.ActiveTime.Presentation;
using DustInTheWind.ActiveTime.Presentation.Commands;
using DustInTheWind.ActiveTime.Presentation.Services;
using DustInTheWind.ActiveTime.Presentation.Tray.Module;
using DustInTheWind.ActiveTime.Presentation.Tray.ViewModels;
using DustInTheWind.ActiveTime.Presentation.Tray.Views;
using DustInTheWind.ActiveTime.Presentation.ViewModels;
using DustInTheWind.ActiveTime.Presentation.Views;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Timer = DustInTheWind.ActiveTime.Infrastructure.Timer;

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

            // GUI - Commands
            containerBuilder.RegisterType<RefreshCommand>().AsSelf();
            containerBuilder.RegisterType<DeleteCommand>().AsSelf();
            containerBuilder.RegisterType<DecrementDayCommand>().AsSelf();
            containerBuilder.RegisterType<IncrementDateCommand>().AsSelf();
            containerBuilder.RegisterType<CalendarCommand>().AsSelf();
            containerBuilder.RegisterType<ResetCommentsCommand>().AsSelf();
            containerBuilder.RegisterType<SaveCommentsCommand>().AsSelf();

            // GUI - Services
            containerBuilder.RegisterType<AutofacWindowFactory>().As<IWindowFactory>();
            containerBuilder.RegisterType<ApplicationService>().As<IApplicationService>().SingleInstance();
            containerBuilder.RegisterType<ShellNavigator>().As<IShellNavigator>().SingleInstance();
            containerBuilder.RegisterType<DispatcherService>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<ViewModelFactory>().As<IViewModelFactory>();
            containerBuilder.RegisterType<CommandFactory>().As<ICommandFactory>();

            // Register singleton services.
            containerBuilder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            containerBuilder.RegisterType<EventBus>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<InMemoryState>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<StatusInfoService>().As<IStatusInfoService>().SingleInstance();

            // Register services.
            containerBuilder.RegisterType<SystemClock>().As<ISystemClock>();
            containerBuilder.RegisterType<Scribe>().AsSelf();
            containerBuilder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            containerBuilder.RegisterType<AutofacUnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            containerBuilder.RegisterType<Dwarfs>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<Timer>().As<ITimer>();

            // Jobs
            containerBuilder.RegisterType<ScheduledJobs>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<RecorderJob>();

            // MediatR
            Assembly useCasesAssembly = typeof(StartRecordingRequest).Assembly;
            MediatRConfiguration mediatRConfiguration = MediatRConfigurationBuilder.Create(useCasesAssembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .Build();
            containerBuilder.RegisterMediatR(mediatRConfiguration);


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