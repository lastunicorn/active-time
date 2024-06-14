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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using DustInTheWind.ActiveTime.Domain.Presentation;
using DustInTheWind.ActiveTime.Domain.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Domain.Services;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Jobs;
using DustInTheWind.ActiveTime.Presentation.AboutArea;
using DustInTheWind.ActiveTime.Presentation.MainArea;
using DustInTheWind.ActiveTime.Presentation.Tray.Module;

namespace DustInTheWind.ActiveTime.Bootstrapper;

internal class BootstrapperWithAutofac
{
    private IContainer container;

    public void Run()
    {
        ConfigureServices();
        ConfigureJobs();
        ConfigureGuiShells();

        StartJobs();

        IApplicationService applicationService = container.Resolve<IApplicationService>();
        applicationService.Start();
    }

    private void ConfigureServices()
    {
        ContainerBuilder containerBuilder = new();
        ServicesConfiguration.Configure(containerBuilder);
        container = containerBuilder.Build();
    }

    private void ConfigureJobs()
    {
        JobCollection jobCollection = container.Resolve<JobCollection>();

        Assembly[] assemblies = new[]
        {
            typeof(RecorderJob).Assembly,
            typeof(TrayIconJob).Assembly
        };

        IEnumerable<IJob> jobs = assemblies
            .SelectMany(CreateJobs);

        foreach (IJob job in jobs)
            jobCollection.Add(job);
    }

    private IEnumerable<IJob> CreateJobs(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && typeof(IJob).IsAssignableFrom(x))
            .Select(x => container.Resolve(x))
            .OfType<IJob>();
    }

    private void ConfigureGuiShells()
    {
        IShellNavigator shellNavigator = container.Resolve<IShellNavigator>();

        // Register shells in the shell navigator. (Needed for shell navigation.)
        shellNavigator.RegisterShell(new ShellInfo(ShellNames.MainShell, typeof(MainWindow)));
        shellNavigator.RegisterShell(new ShellInfo(ShellNames.MessageShell, typeof(MessageWindow), ShellNames.MainShell));
        shellNavigator.RegisterShell(new ShellInfo(ShellNames.AboutShell, typeof(AboutWindow), ShellNames.MainShell));
    }

    private void StartJobs()
    {
        JobCollection jobCollection = container.Resolve<JobCollection>();
        jobCollection.StartAll();
    }
}