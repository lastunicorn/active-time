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

using System.ComponentModel;
using DustInTheWind.ActiveTime.Presentation.Tray.Properties;
using DustInTheWind.ActiveTime.Presentation.Tray.ViewModels;

namespace DustInTheWind.ActiveTime.Presentation.Tray.Views;

public partial class TrayIconView : Component, ITrayIconView
{
    public TrayIconPresenter Presenter { get; }

    public TrayIconView(TrayIconPresenter presenter)
    {
        Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

        InitializeComponent();

        toolStripMenuItemShow.Command = presenter.ShowCommand;
        toolStripMenuItemStart.Command = presenter.StartRecorderCommand;
        toolStripMenuItemStop.Command = presenter.StopRecorderCommand;
        toolStripMenuItemStopAndDelete.Command = presenter.StopRecorderCommand;
        toolStripMenuItemStopAndDelete.CommandParameter = true;
        toolStripMenuItemAbout.Command = presenter.AboutCommand;
        toolStripMenuItemExit.Command = presenter.ExitCommand;

        presenter.View = this;
    }

    public bool Visible
    {
        set => notifyIcon1.Visible = value;
    }

    public TrayIconState IconState
    {
        set
        {
            switch (value)
            {
                case TrayIconState.On:
                    notifyIcon1.Icon = Resources.tray_on;
                    break;

                case TrayIconState.Off:
                    notifyIcon1.Icon = Resources.tray_off;
                    break;

                case TrayIconState.Unknown:
                default:
                    // todo: create an unknown state icon
                    notifyIcon1.Icon = Resources.tray_off;
                    break;
            }
        }
    }

    private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            Presenter.ShowCommand.Execute(null);
        }
    }
}