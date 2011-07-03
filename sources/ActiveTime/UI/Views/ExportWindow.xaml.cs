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
using System.Windows;
using DustInTheWind.ActiveTime.UI.IViews;
using DustInTheWind.ActiveTime.UI.Models;
using DustInTheWind.ActiveTime.UI.Controllers;

namespace DustInTheWind.ActiveTime.UI.Views
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : WindowBase, IExportView
    {
        private ExportPresenter presenter;

        public ExportWindow(ActiveTimeApplication activeTimeApplication)
        {
            InitializeComponent();

            presenter = new ExportPresenter(this, activeTimeApplication);
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            presenter.ExportButtonClicked();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            presenter.BrowseButtonClicked();
        }

        public string RequestNewExportFileName()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Csv Files (.csv)|*.csv";

            if (dlg.ShowDialog() == true)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            presenter.WindowLoaded();
        }

        public ExportModel Model
        {
            set { DataContext = value; }
        }

        private void textBoxYear_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }

        private void textBoxMonth_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
