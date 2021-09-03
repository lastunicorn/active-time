using System;
using System.Reflection;
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
using DustInTheWind.ActiveTime.Presentation.Views;
using DustInTheWind.ActiveTime.Recording.Module.Services;
using DustInTheWind.ActiveTime.TrayGui.Module;
using DustInTheWind.ActiveTime.TrayGui.ViewModels;
using DustInTheWind.ActiveTime.TrayGui.Views;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.ActiveTime
{
    internal class BootstrapperWithMicrosoftDi
    {
        private IServiceProvider serviceProvider;

        public BootstrapperWithMicrosoftDi()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            // Database
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

            // GUI - Tray Icon
            serviceCollection.AddTransient<TrayIconModule2>();
            serviceCollection.AddTransient<TrayIconView>();
            serviceCollection.AddTransient<TrayIconPresenter>();

            // GUI - Services
            serviceCollection.AddTransient<IWindowFactory, MicrosoftDiWindowFactory>();
            serviceCollection.AddSingleton<IApplicationService, ApplicationService>();
            serviceCollection.AddSingleton<IShellNavigator, ShellNavigator>();
            serviceCollection.AddSingleton<DispatcherService>();

            // Register singleton services.
            serviceCollection.AddSingleton<ILogger, Logger>();
            serviceCollection.AddSingleton<CurrentDay>();
            serviceCollection.AddSingleton<EventBus>();
            serviceCollection.AddSingleton<InMemoryState>();
            serviceCollection.AddSingleton<IStatusInfoService, StatusInfoService>();

            // Register services.
            serviceCollection.AddTransient<ISystemClock, SystemClock>();
            serviceCollection.AddTransient<ScribeEx>();
            serviceCollection.AddTransient<IConfigurationService, ConfigurationService>();
            //serviceCollection.AddTransient<IUnitOfWorkFactory, AutofacUnitOfWorkFactory>();
            serviceCollection.AddSingleton<Dwarfs>();

            // Jobs
            serviceCollection.AddSingleton<ScheduledJobs>();
            serviceCollection.AddTransient<RecorderJob>();

            // MediatR
            Assembly useCasesAssembly = typeof(StartRecordingRequest).Assembly;
            serviceCollection.AddMediatR(useCasesAssembly);
        }

        public void Run()
        {
            RegisterJobs();
            RegisterGuiShells();

            InitializeModules();

            IApplicationService applicationService = serviceProvider.GetService<IApplicationService>();
            applicationService.Start();
        }

        private void RegisterJobs()
        {
            ScheduledJobs scheduledJobs = serviceProvider.GetService<ScheduledJobs>();
            RecorderJob recorderJob = serviceProvider.GetService<RecorderJob>();
            scheduledJobs.Add(recorderJob);
        }

        private void RegisterGuiShells()
        {
            IShellNavigator shellNavigator = serviceProvider.GetService<IShellNavigator>();

            // Register shells in the shell navigator. (Needed for shell navigation.)
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.MainShell, typeof(MainWindow)));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.MessageShell, typeof(MessageWindow), ShellNames.MainShell));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.AboutShell, typeof(AboutWindow), ShellNames.MainShell));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.OverviewShell, typeof(OverviewWindow), ShellNames.MainShell));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.CalendarShell, typeof(CalendarWindow), ShellNames.MainShell));
        }

        private void InitializeModules()
        {
            TrayIconModule2 trayIconModule = serviceProvider.GetService<TrayIconModule2>();
            trayIconModule.Initialize();
        }
    }
}