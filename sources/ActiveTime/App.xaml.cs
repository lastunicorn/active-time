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

using System.Windows;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StartRecording;
using DustInTheWind.ActiveTime.Infrastructure.Wpf;
using DustInTheWind.ActiveTime.Infrastructure.Wpf.Setup.Autofac;
using DustInTheWind.ActiveTime.Jobs;
using DustInTheWind.ActiveTime.Presentation.Tray;

namespace DustInTheWind.ActiveTime;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        CustomApplication customApplication = ApplicationBuilder.Create()
            .UseStartUpGuard(config =>
            {
                config.Name = "DustInTheWind.ActiveTime";
            })
            .ConfigureServices(DependenciesSetup.Configure)
            .RegisterJobs(typeof(RecorderJob).Assembly)
            .RegisterGuiShells(ShellSetup.EnumerateShellInfo)
            .RegisterUseCases(typeof(StartRecordingRequest).Assembly)
            .UseTrayIcon<TrayIcon>()
            .Build();

        customApplication.Start();
    }
}