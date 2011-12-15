using System;
using DustInTheWind.ActiveTime.Common;

namespace DustInTheWind.ActiveTime.MainModule.Services
{
    class CurrentTimeProvider : ITimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}
