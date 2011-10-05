using System;
using DustInTheWind.ActiveTime.Common.Recording;

namespace DustInTheWind.ActiveTime.Common
{
    interface IRecorder
    {
        TimeSpan? GetTimeFromLastStop();
        void Stamp();
        event EventHandler Stamped;
        event EventHandler Stamping;
        void Start();
        event EventHandler Started;
        RecorderState State { get; }
        void Stop(bool deleteLastRecord = false);
        event EventHandler Stopped;
    }
}
