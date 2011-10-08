using System;
using DustInTheWind.ActiveTime.Common.Recording;

namespace DustInTheWind.ActiveTime.Common
{
    public interface IRecorder
    {
        //bool IsStarted { get; set; }

        //event EventHandler IsStartedChanged;
        



        RecorderState State { get; }

        event EventHandler Started;
        event EventHandler Stopped;
        event EventHandler Stamped;
        event EventHandler Stamping;

        void Start();
        //void Stamp();
        void Stop(bool deleteLastRecord = false);

        //TimeSpan? GetTimeFromLastStop();
    }
}
