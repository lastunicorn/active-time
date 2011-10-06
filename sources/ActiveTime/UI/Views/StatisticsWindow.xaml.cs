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

using System.Windows;
using System.Windows.Controls;
using DustInTheWind.ActiveTime.UI.Controllers;
using DustInTheWind.ActiveTime.UI.IViews;
using DustInTheWind.ActiveTime.UI.Models;

namespace DustInTheWind.ActiveTime.UI.Views
{
    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : WindowBase, IStatisticsView
    {
        private StatisticsPresenter presenter;

        public StatisticsWindow()
        {
            InitializeComponent();

            presenter = new StatisticsPresenter(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            presenter.WindowLoaded();
        }

        public StatisticsModel Model
        {
            set { DataContext = value; }
        }

        private void textBoxYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            presenter.YearValueChanged();
        }

        private void textBoxMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            presenter.MonthChanged();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
