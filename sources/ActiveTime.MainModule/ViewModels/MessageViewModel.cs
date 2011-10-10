using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.ShellNavigation;

namespace DustInTheWind.ActiveTime.MainModule.ViewModels
{
    public class MessageViewModel : ViewModelBase, IShell
    {
        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged("Message");
            }
        }

        private Dictionary<string, object> navigationParameters;
        public Dictionary<string, object> NavigationParameters
        {
            get { return navigationParameters; }
            set
            {
                navigationParameters = value;
                UpdateTextFromNavigationParameters();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageViewModel"/> class.
        /// </summary>
        public MessageViewModel()
        {
            message = "No message for now.\n\nPlease go back to what you were doing.";
        }

        private void UpdateTextFromNavigationParameters()
        {
            if (navigationParameters != null && navigationParameters.ContainsKey("Text") && navigationParameters["Text"] is string)
            {
                Message = navigationParameters["Text"] as string;
            }
        }
    }
}
