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
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItemShow = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItemStart = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemStop = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemStopAndDelete = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            toolStripTextBoxShow = new System.Windows.Forms.ToolStripTextBox();
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            contextMenuStrip1.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItemShow, toolStripSeparator1, toolStripMenuItemStart, toolStripMenuItemStop, toolStripMenuItemStopAndDelete, toolStripSeparator2, toolStripMenuItemAbout, toolStripSeparator3, toolStripMenuItemExit });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(215, 154);
            // 
            // toolStripMenuItemShow
            // 
            toolStripMenuItemShow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            toolStripMenuItemShow.Name = "toolStripMenuItemShow";
            toolStripMenuItemShow.Size = new System.Drawing.Size(214, 22);
            toolStripMenuItemShow.Text = "&Show";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(211, 6);
            // 
            // toolStripMenuItemStart
            // 
            toolStripMenuItemStart.Name = "toolStripMenuItemStart";
            toolStripMenuItemStart.Size = new System.Drawing.Size(214, 22);
            toolStripMenuItemStart.Text = "St&art";
            // 
            // toolStripMenuItemStop
            // 
            toolStripMenuItemStop.Name = "toolStripMenuItemStop";
            toolStripMenuItemStop.Size = new System.Drawing.Size(214, 22);
            toolStripMenuItemStop.Text = "St&op";
            // 
            // toolStripMenuItemStopAndDelete
            // 
            toolStripMenuItemStopAndDelete.Name = "toolStripMenuItemStopAndDelete";
            toolStripMenuItemStopAndDelete.Size = new System.Drawing.Size(214, 22);
            toolStripMenuItemStopAndDelete.Text = "Stop and delete last record";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(211, 6);
            // 
            // toolStripMenuItemAbout
            // 
            toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            toolStripMenuItemAbout.Size = new System.Drawing.Size(214, 22);
            toolStripMenuItemAbout.Text = "&About";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(211, 6);
            // 
            // toolStripMenuItemExit
            // 
            toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            toolStripMenuItemExit.Size = new System.Drawing.Size(214, 22);
            toolStripMenuItemExit.Text = "E&xit";
            // 
            // toolStripTextBoxShow
            // 
            toolStripTextBoxShow.Margin = new System.Windows.Forms.Padding(1);
            toolStripTextBoxShow.Name = "toolStripTextBoxShow";
            toolStripTextBoxShow.Size = new System.Drawing.Size(100, 23);
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = "Active Time";
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            contextMenuStrip1.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxShow;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStop;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStopAndDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
