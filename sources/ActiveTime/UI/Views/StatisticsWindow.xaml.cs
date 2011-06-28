using System;
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

        public StatisticsWindow(ActiveTimeApplication activeTimeApplication)
        {
            InitializeComponent();

            presenter = new StatisticsPresenter(this, activeTimeApplication);
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
