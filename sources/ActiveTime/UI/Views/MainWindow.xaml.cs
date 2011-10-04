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

using System.Collections;
using System.ComponentModel;
using System.Windows;
using DustInTheWind.ActiveTime.UI.IViews;
using System;
using DustInTheWind.ActiveTime.UI.Controllers;
using DustInTheWind.ActiveTime.UI.Models;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Persistence.Repositories;

namespace DustInTheWind.ActiveTime.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase, IMainView
    {
        private MainPresenter presenter;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow(ActiveTimeApplication activeTimeApplication)
        {
            InitializeComponent();

            presenter = new MainPresenter(this, Resources["model"] as MainModel, activeTimeApplication);
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            presenter.ViewLoaded();
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            presenter.ButtonRefreshClicked();
        }

        private void datePicker1_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            presenter.DateChanged();
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !presenter.WindowClosing();
        }

        private void buttonComments_Click(object sender, RoutedEventArgs e)
        {
            presenter.ButtonCommentsClicked();

            if (datePicker1.SelectedDate != null)
            {
                CommentsWindow window = new CommentsWindow(new DayCommentRepository(), datePicker1.SelectedDate.Value);

                window.Owner = this;
                window.ShowDialog();
            }
        }

        private void menuItemNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemDeleteClicked();
        }

        private void menuItemMerge_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemSplit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemExitClicked();
        }

        public TrayIconManager TrayIconManager
        {
            set
            {
                presenter.TrayIconManager = value;
            }
        }


        public IList SelectedRecords
        {
            get { return listBox1.SelectedItems; }
        }

        public bool Confirm(string text, string caption)
        {
            return MessageBox.Show(this, text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes;
        }


        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        public bool AllowClose
        {
            get { return presenter.AllowClose; }
            set { presenter.AllowClose = value; }
        }

        private void menuItemExport_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemExportClicked();
        }

        public void ShowExportWindow(ActiveTimeApplication activeTimeApplication)
        {
            ExportWindow window = new ExportWindow(activeTimeApplication);
            window.Owner = this;
            window.ShowDialog();
        }

        private void menuItemStatistics_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemStatisticsClicked();
        }

        public void ShowStatisticsWindow(ActiveTimeApplication activeTimeApplication)
        {
            StatisticsWindow window = new StatisticsWindow(activeTimeApplication);
            window.Owner = this;
            window.ShowDialog();
        }

        private void menuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemAboutClicked();
        }

        public void ShowAboutWindow()
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        }
    }
}
