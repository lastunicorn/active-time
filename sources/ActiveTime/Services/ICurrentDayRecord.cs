using System;
using DustInTheWind.ActiveTime.Common.Recording;

namespace DustInTheWind.ActiveTime.Services
{
    public interface ICurrentDayRecord
    {
        DayRecord Value { get; }
        event EventHandler ValueChanged;
        void Update();
    }
}