using System;

namespace DustInTheWind.ActiveTime.Common
{
    public class InMemoryState
    {
        public int? CurrentTimeRecordId { get; set; }

        public DateTime? CurrentDate { get; set; }
        
        public string Comments { get; set; }
    }
}