using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using System.Reflection;

namespace DustInTheWind.ActiveTime.MainGuiModule.ViewModels
{
    public class MainWindowModel : ViewModelBase
    {
        private string windowTitle;
        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                windowTitle = value;
                NotifyPropertyChanged("WindowTitle");
            }
        }

        public MainWindowModel()
        {
            Assembly assembly = Assembly.GetEntryAssembly();

            windowTitle = "ActiveTime " + assembly.GetName().Version.ToString(2);
        }
    }
}
