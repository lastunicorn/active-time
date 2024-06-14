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

public class TrayIconJob : JobBase
{
    private readonly TrayIconView trayIconView;
    private readonly IApplicationService applicationService;

    public override string Id { get; } = "Tray Icon";

    public TrayIconJob(TrayIconView trayIconView, IApplicationService applicationService)
    {
        this.trayIconView = trayIconView ?? throw new ArgumentNullException(nameof(trayIconView));
        this.applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
    }

    protected override void DoStart()
    {
        trayIconView.Presenter.Show();

        applicationService.Exiting += HandleApplicationServiceExiting;
    }

    protected override void DoStop()
    {
        applicationService.Exiting -= HandleApplicationServiceExiting;

        trayIconView.Presenter.Hide();
    }

    private void HandleApplicationServiceExiting(object sender, EventArgs e)
    {
        trayIconView.Presenter.Hide();
    }
}