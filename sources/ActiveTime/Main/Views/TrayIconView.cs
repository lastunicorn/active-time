using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace DustInTheWind.ActiveTime.Main.Services
{
    partial class TrayIconView : Component
    {
        private Icon iconOn;
        private Icon iconOff;

        private ITrayIconService trayIconService;

        public TrayIconView(ITrayIconService trayIconService)
        {
            if (trayIconService == null)
                throw new ArgumentNullException("trayIconService");

            this.trayIconService = trayIconService;

            InitializeComponent();

            iconOn = DustInTheWind.ActiveTime.Properties.Resources.tray_on;
            iconOff = DustInTheWind.ActiveTime.Properties.Resources.tray_off;

            notifyIcon1.Icon = iconOn;

            trayIconService.IconStateChanged += new EventHandler(trayIconService_IconStateChanged);
            trayIconService.IconVisibleChanged += new EventHandler(trayIconService_IconVisibileChanged);

            this.toolStripMenuItemShow.Click += new System.EventHandler(this.toolStripMenuItemShow_Click);
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            this.toolStripMenuItemStart.Click += new EventHandler(toolStripMenuItemStart_Click);
            this.toolStripMenuItemStop.Click += new EventHandler(toolStripMenuItemStop_Click);
            this.toolStripMenuItemStopAndDelete.Click += new EventHandler(toolStripMenuItemStopAndDelete_Click);
        }

        void trayIconService_IconVisibileChanged(object sender, EventArgs e)
        {
            if (trayIconService.IconVisible)
                ShowIcon();
            else
                HideIcon();
        }

        private void trayIconService_IconStateChanged(object sender, EventArgs e)
        {
            switch (trayIconService.IconState)
            {
                case IconState.On:
                    SetIconOn();
                    break;

                case IconState.Off:
                    SetIconOff();
                    break;

                default:
                    break;
            }
        }

        private void toolStripMenuItemStopAndDelete_Click(object sender, EventArgs e)
        {
            trayIconService.RaiseStopAndDeleteClicked();
        }

        private void toolStripMenuItemStop_Click(object sender, EventArgs e)
        {
            trayIconService.RaiseStopClicked();
        }

        private void toolStripMenuItemStart_Click(object sender, EventArgs e)
        {
            trayIconService.RaiseStartClicked();
        }

        private void ShowIcon()
        {
            notifyIcon1.Visible = true;
        }

        private void HideIcon()
        {
            notifyIcon1.Visible = false;
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            trayIconService.RaiseExitClicked();
        }

        private void toolStripMenuItemShow_Click(object sender, System.EventArgs e)
        {
            trayIconService.RaiseShowClicked();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                trayIconService.RaiseShowClicked();
            }
        }

        private void SetIconOn()
        {
            notifyIcon1.Icon = iconOn;
        }

        private void SetIconOff()
        {
            notifyIcon1.Icon = iconOff;
        }

        public bool StartEnabled
        {
            set { toolStripMenuItemStart.Enabled = value; }
        }

        public bool StopEnabled
        {
            set
            {
                toolStripMenuItemStop.Enabled = value;
                toolStripMenuItemStopAndDelete.Enabled = value;
            }
        }
    }
}
