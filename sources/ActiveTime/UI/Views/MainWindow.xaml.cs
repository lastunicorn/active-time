using System.Collections;
using System.ComponentModel;
using System.Windows;
using DustInTheWind.ActiveTime.UI.IViews;
using System;
using DustInTheWind.ActiveTime.UI.Controllers;
using DustInTheWind.ActiveTime.UI.Models;

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
        internal MainWindow(ActiveTimeApplication activeTimeApplication)
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
                CommentsWindow window = new CommentsWindow(new Dal(), datePicker1.SelectedDate.Value);

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

        private void menuItemExportFull_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemExportFullClicked();
        }

        private void menuItemExportNormal_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemExportNormalClicked();
        }

        private void menuItemExportMinimal_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemExportMinimalClicked();
        }

        private void menuItemExportSafe_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemExportSafeClicked();
        }

        public void ShowExportWindow(ActiveTimeApplication activeTimeApplication)
        {
            ExportWindow window = new ExportWindow(activeTimeApplication);
            window.ShowDialog();
        }

        private void menuItemStatistics_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemStatisticsClicked();
        }

        public void ShowStatisticsWindow(ActiveTimeApplication activeTimeApplication)
        {
            StatisticsWindow window = new StatisticsWindow(activeTimeApplication);
            window.ShowDialog();
        }

        private void menuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            presenter.MenuItemAboutClicked();
        }

        public void ShowAboutWindow()
        {
            AboutWindow window = new AboutWindow();
            window.ShowDialog();
        }
    }
}
