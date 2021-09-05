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

using DustInTheWind.ActiveTime.Presentation.Tray.CustomControls;

namespace DustInTheWind.ActiveTime.Presentation.Tray.Views
{
    public partial class TrayIconView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemShow = new CustomMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemStart = new CustomMenuItem();
            this.toolStripMenuItemStop = new CustomMenuItem();
            this.toolStripMenuItemStopAndDelete = new CustomMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExit = new CustomMenuItem();
            this.toolStripTextBoxShow = new System.Windows.Forms.ToolStripTextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStripMenuItemAbout = new CustomMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemShow,
            this.toolStripSeparator1,
            this.toolStripMenuItemStart,
            this.toolStripMenuItemStop,
            this.toolStripMenuItemStopAndDelete,
            this.toolStripSeparator2,
            this.toolStripMenuItemAbout,
            this.toolStripSeparator3,
            this.toolStripMenuItemExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(215, 154);
            // 
            // toolStripMenuItemShow
            // 
            this.toolStripMenuItemShow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.toolStripMenuItemShow.Name = "toolStripMenuItemShow";
            this.toolStripMenuItemShow.Size = new System.Drawing.Size(214, 22);
            this.toolStripMenuItemShow.Text = "&Show";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(211, 6);
            // 
            // toolStripMenuItemStart
            // 
            this.toolStripMenuItemStart.Name = "toolStripMenuItemStart";
            this.toolStripMenuItemStart.Size = new System.Drawing.Size(214, 22);
            this.toolStripMenuItemStart.Text = "St&art";
            // 
            // toolStripMenuItemStop
            // 
            this.toolStripMenuItemStop.Name = "toolStripMenuItemStop";
            this.toolStripMenuItemStop.Size = new System.Drawing.Size(214, 22);
            this.toolStripMenuItemStop.Text = "St&op";
            // 
            // toolStripMenuItemStopAndDelete
            // 
            this.toolStripMenuItemStopAndDelete.Name = "toolStripMenuItemStopAndDelete";
            this.toolStripMenuItemStopAndDelete.Size = new System.Drawing.Size(214, 22);
            this.toolStripMenuItemStopAndDelete.Text = "Stop and delete last record";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(211, 6);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(214, 22);
            this.toolStripMenuItemExit.Text = "E&xit";
            // 
            // toolStripTextBoxShow
            // 
            this.toolStripTextBoxShow.Margin = new System.Windows.Forms.Padding(1);
            this.toolStripTextBoxShow.Name = "toolStripTextBoxShow";
            this.toolStripTextBoxShow.Size = new System.Drawing.Size(100, 23);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "Active Time";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(214, 22);
            this.toolStripMenuItemAbout.Text = "&About";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(211, 6);
            this.contextMenuStrip1.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxShow;
        private CustomMenuItem toolStripMenuItemExit;
        private CustomMenuItem toolStripMenuItemShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private CustomMenuItem toolStripMenuItemStop;
        private CustomMenuItem toolStripMenuItemStart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private CustomMenuItem toolStripMenuItemStopAndDelete;
        private CustomMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
