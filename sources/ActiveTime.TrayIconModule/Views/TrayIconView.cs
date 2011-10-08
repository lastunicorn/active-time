// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.TrayIconModule.ViewModels;

namespace DustInTheWind.ActiveTime.TrayIconModule.Views
{
    partial class TrayIconView : Component, ITrayIconView
    {
        private TrayIconPresenter presenter;

        public TrayIconView(TrayIconPresenter presenter)
        {
            if (presenter == null)
                throw new ArgumentNullException("presenter");

            this.presenter = presenter;

            InitializeComponent();

            this.toolStripMenuItemShow.Click += new EventHandler(toolStripMenuItemShow_Click);
            this.toolStripMenuItemExit.Click += new EventHandler(toolStripMenuItemExit_Click);
            this.toolStripMenuItemStart.Click += new EventHandler(toolStripMenuItemStart_Click);
            this.toolStripMenuItemStop.Click += new EventHandler(toolStripMenuItemStop_Click);
            this.toolStripMenuItemStopAndDelete.Click += new EventHandler(toolStripMenuItemStopAndDelete_Click);

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

        private delegate void SetBoolDelegate(bool value);

        public bool StartMenuItemEnabled
        {
            set
            {
                if (contextMenuStrip1.InvokeRequired)
                {
                    contextMenuStrip1.Invoke(new SetBoolDelegate(v => StartMenuItemEnabled = v), value);
                }
                else
                {
                    toolStripMenuItemStart.Enabled = value;
                }
            }
        }

        public bool StopMenuItemEnabled
        {
            set
            {
                if (contextMenuStrip1.InvokeRequired)
                {
                    contextMenuStrip1.Invoke(new SetBoolDelegate(v => StopMenuItemEnabled = v), value);
                }
                else
                {
                    toolStripMenuItemStop.Enabled = value;
                    toolStripMenuItemStopAndDelete.Enabled = value;
                }
            }
        }


        private void toolStripMenuItemStopAndDelete_Click(object sender, EventArgs e)
        {
            presenter.StopAndDeleteClicked();
        }

        private void toolStripMenuItemStop_Click(object sender, EventArgs e)
        {
            presenter.StopClicked();
        }

        private void toolStripMenuItemStart_Click(object sender, EventArgs e)
        {
            presenter.StartClicked();
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            presenter.ExitClicked();
        }

        private void toolStripMenuItemShow_Click(object sender, System.EventArgs e)
        {
            presenter.ShowClicked();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                presenter.LeftDoubleClicked();
            }
        }
    }
}
