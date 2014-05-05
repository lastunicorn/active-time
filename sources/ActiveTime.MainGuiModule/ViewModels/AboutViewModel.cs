using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DustInTheWind.ActiveTime.Common.UI;

namespace DustInTheWind.ActiveTime.MainGuiModule.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public string Version { get; set; }

        public AboutViewModel()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            Version = "Version " + assembly.GetName().Version.ToString(2);
        }
    }
}
