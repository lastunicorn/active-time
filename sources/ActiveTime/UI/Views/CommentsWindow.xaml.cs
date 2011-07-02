using System;
using System.Windows;
using System.Windows.Controls;
using DustInTheWind.ActiveTime.UI.Controllers;
using DustInTheWind.ActiveTime.UI.IViews;
using DustInTheWind.ActiveTime.UI.Models;
using DustInTheWind.ActiveTime.Persistence;

namespace DustInTheWind.ActiveTime.UI.Views
{
    /// <summary>
    /// Interaction logic for CommentsWindow.xaml
    /// </summary>
    public partial class CommentsWindow : WindowBase, ICommentsView
    {
        private CommentsPresenter presenter;

        public CommentsModel Model
        {
            set { DataContext = value; }
        }

        public CommentsWindow()
        {
            InitializeComponent();
        }

        internal CommentsWindow(Dal dal, DateTime date)
            : this()
        {
            presenter = new CommentsPresenter(this, dal, date);

            datePickerDate.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            presenter.WindowLoaded();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            presenter.SaveButtonClicked();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            presenter.CancelButtonClicked();
        }

        private void buttonApply_Click(object sender, RoutedEventArgs e)
        {
            presenter.ApplyButtonClicked();
        }

        public void CloseWithCancel()
        {
            DialogResult = false;
        }

        public void CloseWithOk()
        {
            DialogResult = true;
        }
    }
}
