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

using System;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Domain.Services;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Presentation.Tray.Views;

namespace DustInTheWind.ActiveTime.Presentation.Tray.Module;

public class TrayIconJob : IJob
{
    private readonly TrayIconView trayIconView;

    public string Id { get; } = "Tray Icon";

    public JobState State { get; private set; }

    public TrayIconJob(TrayIconView trayIconView, IApplicationService applicationService)
    {
        this.trayIconView = trayIconView ?? throw new ArgumentNullException(nameof(trayIconView));

        applicationService.Exiting += HandleApplicationServiceExiting;
    }

    private void HandleApplicationServiceExiting(object sender, EventArgs e)
    {
        trayIconView.Presenter.Hide();
    }

    public Task Start()
    {
        trayIconView.Presenter.Show();
        State = JobState.Running;

        return Task.CompletedTask;
    }

    public Task Stop()
    {
        trayIconView.Presenter.Hide();
        State = JobState.Stopped;

        return Task.CompletedTask;
    }
}