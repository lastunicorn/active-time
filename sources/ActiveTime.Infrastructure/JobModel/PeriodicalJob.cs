using System;
using System.Threading.Tasks;

namespace DustInTheWind.ActiveTime.Infrastructure.JobModel
{
    public abstract class PeriodicalJob : JobBase
    {
        public bool RunOnStart { get; set; }

        public TimeSpan RunInterval
        {
            get => Timer.Interval;
            set
            {
                lock (StateSynchronizer)
                {
                    Timer.Interval = value;
                }
            }
        }

        protected PeriodicalJob(ITimer timer)
            : base(timer)
        {
        }

        protected override async Task OnStarted()
        {
            await base.OnStarted();

            if (RunOnStart)
                await Execute();
        }
    }
}