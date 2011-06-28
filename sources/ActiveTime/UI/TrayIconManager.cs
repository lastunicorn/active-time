using System;
using System.ComponentModel;
using System.Drawing;
using DustInTheWind.ActiveTime.UI.IViews;

namespace DustInTheWind.ActiveTime
{
    public partial class TrayIconManager : Component, ITrayIconView
    {
        private Icon iconOn;
        private Icon iconOff;

        public event EventHandler ExitClicked;

        protected virtual void OnExitClicked(EventArgs e)
        {
            if (ExitClicked != null)
                ExitClicked(this, e);
        }

        public event EventHandler ShowClicked;

        protected virtual void OnShowClicked(EventArgs e)
        {
            if (ShowClicked != null)
                ShowClicked(this, e);
        }

        public event EventHandler StartClicked;

        protected virtual void OnStartClicked(EventArgs e)
        {
            if (StartClicked != null)
                StartClicked(this, e);
        }

        public event EventHandler StopClicked;

        protected virtual void OnStopClicked(EventArgs e)
        {
            if (StopClicked != null)
                StopClicked(this, e);
        }

        public event EventHandler StopAndDeleteClicked;

        protected virtual void OnStopAndDeleteClicked(EventArgs e)
        {
            if (StopAndDeleteClicked != null)
                StopAndDeleteClicked(this, e);
        }

        public TrayIconManager()
            : this(null)
        {
        }

        public TrayIconManager(IContainer container)
        {
            if (container != null)
                container.Add(this);

            InitializeComponent();

            iconOn = DustInTheWind.ActiveTime.Properties.Resources.tray_on;
            iconOff = DustInTheWind.ActiveTime.Properties.Resources.tray_off;

            notifyIcon1.Icon = iconOn;

            this.toolStripMenuItemShow.Click += new System.EventHandler(this.toolStripMenuItemShow_Click);
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            this.toolStripMenuItemStart.Click += new EventHandler(toolStripMenuItemStart_Click);
            this.toolStripMenuItemStop.Click += new EventHandler(toolStripMenuItemStop_Click);
            this.toolStripMenuItemStopAndDelete.Click += new EventHandler(toolStripMenuItemStopAndDelete_Click);
        }

        private void toolStripMenuItemStopAndDelete_Click(object sender, EventArgs e)
        {
            OnStopAndDeleteClicked(EventArgs.Empty);
        }

        private void toolStripMenuItemStop_Click(object sender, EventArgs e)
        {
            OnStopClicked(EventArgs.Empty);
        }

        private void toolStripMenuItemStart_Click(object sender, EventArgs e)
        {
            OnStartClicked(EventArgs.Empty);
        }

        public void ShowIcon()
        {
            notifyIcon1.Visible = true;
        }

        public void HideIcon()
        {
            notifyIcon1.Visible = false;
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            OnExitClicked(EventArgs.Empty);
        }

        private void toolStripMenuItemShow_Click(object sender, System.EventArgs e)
        {
            OnShowClicked(EventArgs.Empty);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                OnShowClicked(EventArgs.Empty);
            }
        }

        public void SetIconOn()
        {
            notifyIcon1.Icon = iconOn;
        }

        public void SetIconOff()
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
