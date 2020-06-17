// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DustInTheWind.ActiveTime.TrayIconModule.ViewModels;

namespace DustInTheWind.ActiveTime.TrayIconModule.Views
{
    partial class TrayIconView : Component, ITrayIconView
    {
        private readonly TrayIconPresenter presenter;

        public TrayIconView(TrayIconPresenter presenter)
        {
            if (presenter == null) throw new ArgumentNullException(nameof(presenter));

            this.presenter = presenter;

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
            set { notifyIcon1.Visible = value; }
        }

        public Icon Icon
        {
            set { notifyIcon1.Icon = value; }
        }
        
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                presenter.LeftDoubleClicked();
        }
    }
}
