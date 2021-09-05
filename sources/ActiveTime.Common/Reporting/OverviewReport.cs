using System;
using System.Collections.Generic;

namespace DustInTheWind.ActiveTime.Common.Reporting
{
    public class OverviewReport
    {
        public DateTime FirstDay { get; set; }

        public DateTime LastDay { get; set; }

        public List<DayRecord> DayRecords { get; set; }
    }
}