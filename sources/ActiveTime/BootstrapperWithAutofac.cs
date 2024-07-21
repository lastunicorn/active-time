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
using DustInTheWind.ActiveTime.Domain.Presentation;
using DustInTheWind.ActiveTime.Domain.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Domain.Services;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Presentation.AboutArea;
using DustInTheWind.ActiveTime.Presentation.MainArea;

namespace DustInTheWind.ActiveTime;

internal sealed class BootstrapperWithAutofac : IDisposable
{
    private StartupGuard startupGuard;
    private IContainer container;

    public void Run()
    {
        startupGuard = new StartupGuard();

        if (startupGuard.Start())
        {
            ConfigureServices();
            ConfigureJobs();
            ConfigureGuiShells();

            StartJobs();

            IApplicationService applicationService = container.Resolve<IApplicationService>();
            applicationService.Start();
        }
        else
        {
            System.Windows.Application.Current.Shutdown();
        }
    }

    private void ConfigureServices()
    {
        ContainerBuilder containerBuilder = new();
        DependenciesSetup.Configure(containerBuilder);
        container = containerBuilder.Build();
    }

    private void ConfigureJobs()
    {
        JobCollection jobCollection = container.Resolve<JobCollection>();

        IEnumerable<IJob> jobs = container.Resolve<IEnumerable<IJob>>();

        foreach (IJob job in jobs)
            jobCollection.Add(job);
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

    public void Dispose()
    {
        startupGuard?.Dispose();
        container?.Dispose();
    }
}