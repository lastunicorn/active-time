using System;
using System.Windows.Controls;
using DustInTheWind.ActiveTime.Main.ViewModels;

namespace DustInTheWind.ActiveTime.Main.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView(MainViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            InitializeComponent();

            Loaded += (s, e) => DataContext = viewModel;
        }

        private void datePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void menuItemNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void menuItemMerge_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void menuItemSplit_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void menuItemDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
