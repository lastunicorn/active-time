using System;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.PresentCurrentDateInfo
{
    public class PresentCurrentDateInfoResponse
    {
        public TimeSpan ActiveTime { get; set; }

        public TimeSpan TotalTime { get; set; }

        public TimeSpan? BeginTime { get; set; }

        public TimeSpan? EstimatedEndTime { get; set; }
    }
}