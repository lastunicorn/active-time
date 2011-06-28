using System;
using System.Windows;
using DustInTheWind.ActiveTime.UI.IViews;

namespace DustInTheWind.ActiveTime.UI.Views
{
    public class WindowBase : Window, IViewBase
    {
        public WindowBase()
        {
        }

        #region Display Error Messages

        /// <summary>
        /// Displays the exception in a frendlly way for the user.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> instance containing data about the error.</param>
        public void DisplayError(Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays an informational message to the user.
        /// </summary>
        /// <param name="message">The message Text to be displayed.</param>
        public void DisplayInfoMessage(string message)
        {
            MessageBox.Show(this, message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Displays an error message to the user.
        /// </summary>
        /// <param name="message">The message Text to be displayed.</param>
        public void DisplayErrorMessage(string message)
        {
            MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}
