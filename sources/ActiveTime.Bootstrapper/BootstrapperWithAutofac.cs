using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Application.CurrentDate.ChangeDate;
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
using Timer = DustInTheWind.ActiveTime.Infrastructure.Timer;

namespace DustInTheWind.ActiveTime
{

    //internal class SomeViewModel
    //{
    //    private readonly IMediator mediator;

    //    public RelayCommand Action1 { get; }

    //    public SomeViewModel(IMediator mediator)
    //    {
    //        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    //        Action1 = new RelayCommand(DoAction1);
    //    }

    //    private void DoAction1()
    //    {
    //        Action1Request request = new Action1Request();
    //        mediator.Send(request);
    //    }
    //}

    //public class Action1Request : IRequest
    //{
    //}

    //public class Action1RequestHandler : IRequestHandler<Action1Request>
    //{
    //    public Task<Unit> Handle(Action1Request request, CancellationToken cancellationToken)
    //    {
    //        return Task.FromResult(Unit.Value);
    //    }
    //}






    //public sealed class CustomRequestHandler : IRequestHandler<CustomRequest>, IDisposable
    //{
    //    private readonly IDisposableResource disposableResource;

    //    public CustomRequestHandler(IDisposableResource disposableResource)
    //    {
    //        this.disposableResource = disposableResource ?? throw new ArgumentNullException(nameof(disposableResource));
    //    }

    //    public Task<Unit> Handle(CustomRequest request, CancellationToken cancellationToken)
    //    {
    //        return Task.FromResult(Unit.Value);
    //    }

    //    public void Dispose()
    //    {
    //        disposableResource?.Dispose();
    //    }
    //}

    //public interface IDisposableResource : IDisposable
    //{
    //}

    //public class CustomRequest : IRequest
    //{
    //}







    //    internal class SomeViewModel
    //    {
    //        private readonly IMediator mediator;

    //        public RelayCommand Action1 { get; }

    //        public RelayCommand Action2 { get; }

    //        public SomeViewModel(IMediator mediator)
    //        {
    //            this.mediator = mediator;

    //            Action1 = new RelayCommand(DoAction1);
    //            Action2 = new RelayCommand(DoAction2);
    //        }

    //        private void DoAction1()
    //        {
    //            Action1Request request = new Action1Request();
    //            mediator.Send(request);
    //        }

    //        private void DoAction2()
    //        {
    //            Action2Request request = new Action2Request();
    //            mediator.Send(request);
    //        }
    //    }

    //    public class Action1Request : IRequest { }

    //    public class Action1RequestHandler : IRequestHandler<Action1Request>
    //    {
    //        public Task<Unit> Handle(Action1Request request, CancellationToken cancellationToken)
    //        {
    //            return Task.FromResult(Unit.Value);
    //        }
    //    }

    //    public class Action2Request : IRequest { }

    //    public class Action2RequestHandler : IRequestHandler<Action2Request>
    //    {
    //        public Task<Unit> Handle(Action2Request request, CancellationToken cancellationToken)
    //        {
    //            return Task.FromResult(Unit.Value);
    //        }
    //    }

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
            containerBuilder.RegisterType<CurrentDay>().AsSelf().SingleInstance();
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
            containerBuilder.RegisterType<TrayIconJob>();

            // MediatR
            Assembly useCasesAssembly = typeof(StartRecordingRequest).Assembly;
            containerBuilder.RegisterMediatR(useCasesAssembly);

            containerBuilder.Register<ServiceFactory>(outerContext =>
            {
                ILifetimeScope parentLifetimeScope = outerContext.Resolve<ILifetimeScope>();

                return serviceType => parentLifetimeScope.BeginLifetimeScope().Resolve(serviceType);
            });
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

            TrayIconJob trayIconJob = container.Resolve<TrayIconJob>();
            scheduledJobs.Add(trayIconJob);
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
            //TrayIconModule trayIconModule = container.Resolve<TrayIconModule>();
            //trayIconModule.Initialize();

            ScheduledJobs scheduledJobs = container.Resolve<ScheduledJobs>();
            scheduledJobs.Start(JobNames.TrayIcon);
        }
    }
}