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
using Autofac;
using DustInTheWind.ActiveTime.Infrastructure.JobEngine;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using DustInTheWind.ActiveTime.Infrastructure.Wpf.ShellEngine;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

namespace DustInTheWind.ActiveTime.Infrastructure.Wpf.Setup.Autofac;

public class ApplicationBuilder
{
    private readonly ContainerBuilder containerBuilder;
    private GuardConfiguration guardConfiguration;
    private IEnumerable<ShellInfo> shellInfos;
    private Type trayIconType;

    private ApplicationBuilder()
    {
        containerBuilder = new ContainerBuilder();

        ConfigureDefaultServices();
    }

    public static ApplicationBuilder Create()
    {
        return new ApplicationBuilder();
    }

    private void ConfigureDefaultServices()
    {
        containerBuilder.RegisterType<JobCollection>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<ShellNavigator>().As<IShellNavigator>().SingleInstance();

        containerBuilder.RegisterType<WindowFactory>().As<IWindowFactory>();
    }

    public ApplicationBuilder UseStartUpGuard(Action<GuardConfiguration> action)
    {
        guardConfiguration = new GuardConfiguration
        {
            IsEnabled = true
        };

        action?.Invoke(guardConfiguration);

        if (guardConfiguration.IsEnabled && string.IsNullOrEmpty(guardConfiguration.Name))
            throw new Exception("The guard must have a name.");

        return this;
    }

    public ApplicationBuilder ConfigureServices(Action<ContainerBuilder> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        action(containerBuilder);

        return this;
    }

    public ApplicationBuilder RegisterJobs(params Assembly[] assemblies)
    {
        IEnumerable<Type> jobTypes = assemblies
            .SelectMany(FindJobs);

        foreach (Type jobType in jobTypes)
            containerBuilder.RegisterType(jobType).As<IJob>();

        return this;
    }

    private static IEnumerable<Type> FindJobs(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && typeof(IJob).IsAssignableFrom(x));
    }

    public ApplicationBuilder RegisterGuiShells(Func<IEnumerable<ShellInfo>> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        shellInfos = action();

        return this;
    }

    public ApplicationBuilder RegisterUseCases(params Assembly[] assemblies)
    {
        // MediatR
        MediatRConfiguration mediatRConfiguration = MediatRConfigurationBuilder.Create(assemblies)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
        containerBuilder.RegisterMediatR(mediatRConfiguration);

        // Services
        containerBuilder.RegisterType<EventBus>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<RequestBus>().As<IRequestBus>().SingleInstance();

        return this;
    }

    public ApplicationBuilder UseTrayIcon<TTrayIcon>()
        where TTrayIcon : ITrayIcon
    {
        trayIconType = typeof(TTrayIcon);

        containerBuilder.RegisterType<TTrayIcon>().As<ITrayIcon>();

        return this;
    }

    public CustomApplication Build()
    {
        CustomApplication application = new()
        {
            StartupGuard = CreateStartupGuard()
        };

        containerBuilder.RegisterInstance(application).As<IApplication>().SingleInstance();

        IContainer container = containerBuilder.Build();

        application.TrayIcon = InstantiateTrayIcon(container);
        application.Jobs = InstantiateJobs(container);
        application.ShellNavigator = InstantiateShellNavigator(container);

        return application;
    }

    private StartupGuard CreateStartupGuard()
    {
        if (guardConfiguration.IsEnabled)
        {
            return new StartupGuard
            {
                Name = guardConfiguration.Name,
                IsActiveInDebugMode = guardConfiguration.IsActiveInDebugMode
            };
        }

        return null;
    }

    private static JobCollection InstantiateJobs(IContainer container)
    {
        JobCollection jobCollection = container.Resolve<JobCollection>();

        IEnumerable<IJob> jobs = container.Resolve<IEnumerable<IJob>>();
        jobCollection.AddRange(jobs);

        return jobCollection;
    }

    private IShellNavigator InstantiateShellNavigator(IContainer container)
    {
        IShellNavigator shellNavigator = container.Resolve<IShellNavigator>();

        foreach (ShellInfo shellInfo in shellInfos)
            shellNavigator.RegisterShell(shellInfo);

        return shellNavigator;
    }

    private ITrayIcon InstantiateTrayIcon(IContainer container)
    {
        return trayIconType == null
            ? null
            : container.Resolve<ITrayIcon>();
    }
}