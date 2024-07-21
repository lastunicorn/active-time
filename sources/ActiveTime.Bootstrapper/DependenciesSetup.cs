// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Reflection;
using ActiveTime.Infrastructure.JobModel.Setup.Autofac;
using Autofac;
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Application.Recording2;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StartRecording;
using DustInTheWind.ActiveTime.ConfigurationAccess;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Domain.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Domain.Services;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Jobs;
using DustInTheWind.ActiveTime.LogAccess;
using DustInTheWind.ActiveTime.Persistence.LiteDB;
using DustInTheWind.ActiveTime.Ports.ConfigurationAccess;
using DustInTheWind.ActiveTime.Ports.LogAccess;
using DustInTheWind.ActiveTime.Ports.Persistence;
using DustInTheWind.ActiveTime.Ports.SystemAccess;
using DustInTheWind.ActiveTime.Presentation;
using DustInTheWind.ActiveTime.Presentation.AboutArea;
using DustInTheWind.ActiveTime.Presentation.CalendarArea;
using DustInTheWind.ActiveTime.Presentation.Commands;
using DustInTheWind.ActiveTime.Presentation.MainArea;
using DustInTheWind.ActiveTime.Presentation.MainMenuArea;
using DustInTheWind.ActiveTime.Presentation.OverviewArea;
using DustInTheWind.ActiveTime.Presentation.RecorderArea;
using DustInTheWind.ActiveTime.Presentation.Services;
using DustInTheWind.ActiveTime.Presentation.Tray.Module;
using DustInTheWind.ActiveTime.Presentation.Tray.ViewModels;
using DustInTheWind.ActiveTime.Presentation.Tray.Views;
using DustInTheWind.ActiveTime.SystemAccess;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using ITimer = DustInTheWind.ActiveTime.Infrastructure.ITimer;
using Timer = DustInTheWind.ActiveTime.Infrastructure.Timer;

namespace DustInTheWind.ActiveTime.Bootstrapper;

internal static class DependenciesSetup
{
    public static void Configure(ContainerBuilder containerBuilder)
    {
        // Database
        containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

        // GUI - Tray Icon
        containerBuilder.RegisterType<TrayIconView>().AsSelf();
        containerBuilder.RegisterType<TrayIconPresenter>().AsSelf();

        // GUI - Windows
        containerBuilder.RegisterType<MainWindow>().AsSelf();
        containerBuilder.RegisterType<MainViewModel>().AsSelf();
        containerBuilder.RegisterType<MainMenuViewModel>().AsSelf();
        containerBuilder.RegisterType<StatusInfoViewModel>().AsSelf();
        containerBuilder.RegisterType<OverviewViewModel>().AsSelf();
        containerBuilder.RegisterType<FrontViewModel>().AsSelf();
        containerBuilder.RegisterType<CurrentDateViewModel>().AsSelf();
        containerBuilder.RegisterType<TimeReportViewModel>().AsSelf();
        containerBuilder.RegisterType<CommentsViewModel>().AsSelf();
        containerBuilder.RegisterType<DayRecordsViewModel>().AsSelf();
        containerBuilder.RegisterType<RecorderViewModel>().AsSelf();

        containerBuilder.RegisterType<AboutWindow>().AsSelf();
        containerBuilder.RegisterType<AboutViewModel>().AsSelf();

        // GUI - Commands
        containerBuilder.RegisterType<RefreshCommand>().AsSelf();
        containerBuilder.RegisterType<DeleteCommand>().AsSelf();
        containerBuilder.RegisterType<DecrementDayCommand>().AsSelf();
        containerBuilder.RegisterType<IncrementDateCommand>().AsSelf();
        containerBuilder.RegisterType<ResetCommentsCommand>().AsSelf();
        containerBuilder.RegisterType<SaveCommentsCommand>().AsSelf();
        containerBuilder.RegisterType<ExitCommand>().AsSelf();
        containerBuilder.RegisterType<StartRecorderCommand>().AsSelf();
        containerBuilder.RegisterType<StopRecorderCommand>().AsSelf();

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
        containerBuilder.RegisterType<MediatrRequestBus>().As<IRequestBus>().SingleInstance();
        containerBuilder.RegisterType<CurrentDay>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<StatusInfoService>().AsSelf().SingleInstance();

        // Register services.
        containerBuilder.RegisterType<SystemClock>().As<ISystemClock>();
        containerBuilder.RegisterType<Scribe>().AsSelf();
        containerBuilder.RegisterType<ConfigurationService>().As<IConfigurationService>();
        containerBuilder.RegisterType<Dwarfs>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<Timer>().As<ITimer>();

        // Jobs
        containerBuilder.RegisterType<JobCollection>().AsSelf().SingleInstance();
        containerBuilder.RegisterJobs(new[] {
            typeof(RecorderJob).Assembly,
            typeof(TrayIconJob).Assembly
        });

        // MediatR
        Assembly useCasesAssembly = typeof(StartRecordingRequest).Assembly;
        MediatRConfiguration mediatRConfiguration = MediatRConfigurationBuilder.Create(useCasesAssembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
        containerBuilder.RegisterMediatR(mediatRConfiguration);
    }
}