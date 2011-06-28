using System.Windows;

namespace DustInTheWind.ActiveTime.UI.Views
{
    /// <summary>
    /// Interaction logic for PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : WindowBase
    {
        public PauseWindow()
        {
            InitializeComponent();
        }

        public PauseWindow(string text)
        {
            InitializeComponent();

            textBlockMessage.Text = text;
        }

        private void buttonSnooze_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
