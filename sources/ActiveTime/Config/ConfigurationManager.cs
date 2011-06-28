using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.Config
{
    class ConfigurationManager
    {
        private TimeSpan? snoozeTimes;

        public TimeSpan? SnoozeTimes
        {
            get { return snoozeTimes; }
            set { snoozeTimes = value; }
        }
    }
}
