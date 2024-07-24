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

using Autofac;
using DustInTheWind.ActiveTime.Adapters.ConfigurationAccess;
using DustInTheWind.ActiveTime.Adapters.DataAccess.LiteDB;
using DustInTheWind.ActiveTime.Adapters.LogAccess;
using DustInTheWind.ActiveTime.Adapters.SystemAccess;
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Application.Recording2;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.ConfigurationAccess;
using DustInTheWind.ActiveTime.Ports.DataAccess;
using DustInTheWind.ActiveTime.Ports.LogAccess;
using DustInTheWind.ActiveTime.Ports.SystemAccess;
using DustInTheWind.ActiveTime.Presentation.AboutArea;
using DustInTheWind.ActiveTime.Presentation.CalendarArea;
using DustInTheWind.ActiveTime.Presentation.Commands;
using DustInTheWind.ActiveTime.Presentation.MainArea;
using DustInTheWind.ActiveTime.Presentation.MainMenuArea;
using DustInTheWind.ActiveTime.Presentation.OverviewArea;
using DustInTheWind.ActiveTime.Presentation.RecorderArea;
using DustInTheWind.ActiveTime.Presentation.Tray.ViewModels;
using DustInTheWind.ActiveTime.Presentation.Tray.Views;
using ITimer = DustInTheWind.ActiveTime.Infrastructure.JobEngine.ITimer;
using Timer = DustInTheWind.ActiveTime.Infrastructure.JobEngine.Timer;

namespace DustInTheWind.ActiveTime;

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
        containerBuilder.RegisterType<RefreshCommand>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<DeleteCommand>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<DecrementDayCommand>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<IncrementDateCommand>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<ResetCommentsCommand>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<SaveCommentsCommand>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<ExitCommand>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<StartRecorderCommand>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<StopRecorderCommand>().AsSelf().SingleInstance();

        // Register services.
        containerBuilder.RegisterType<Log>().As<ILog>().SingleInstance();
        containerBuilder.RegisterType<CurrentDay>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<StatusInfoService>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<SystemClock>().As<ISystemClock>();
        containerBuilder.RegisterType<Scribe>().AsSelf();
        containerBuilder.RegisterType<ConfigurationService>().As<IConfigurationService>();
        containerBuilder.RegisterType<Timer>().As<ITimer>();
    }
}