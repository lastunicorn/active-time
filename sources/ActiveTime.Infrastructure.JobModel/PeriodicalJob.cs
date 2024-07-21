// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Threading.Tasks;

namespace DustInTheWind.ActiveTime.Infrastructure.JobModel
{
    public abstract class PeriodicalJob : TimerJobBase
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