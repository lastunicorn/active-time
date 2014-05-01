// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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
using System.Diagnostics;
using System.Threading;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.RecorderModule;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.RecorderModule.Services.RecorderServiceTests
{
    [TestFixture]
    public class InternalTimerTests
    {
        private Mock<IScribe> scribMock;
        private Mock<IApplicationService> applicationService;

        /// <summary>
        /// Time interval in milliseconds that is used in tests for the StampingInterval of the RecorderService.
        /// This value is used by the RecorderService to configure its internal timer.
        /// </summary>
        private const int STAMPING_INTERVAL = 100;

        /// <summary>
        /// Time in milliseconds that is used in tests to wait for the internal timer to do its job.
        /// </summary>
        private const int WAIT_TIME_FOR_TIMER_ACTION = 200;

        [SetUp]
        public void SetUp()
        {
            scribMock = new Mock<IScribe>();
            applicationService = new Mock<IApplicationService>();
        }

        private RecorderService CreateRecorderService()
        {
            return new RecorderService(scribMock.Object, applicationService.Object);
        }

        [Test]
        public void InternalTimer_stamps_at_correct_time_interval()
        {
            Stopwatch stopwatch = new Stopwatch();
            ManualResetEvent semaphore = new ManualResetEvent(false);

            RecorderService recorderService = CreateRecorderService();
            recorderService.StampingInterval = TimeSpan.FromMilliseconds(STAMPING_INTERVAL);
            scribMock.Setup(x => x.Stamp()).Callback(() =>
            {
                stopwatch.Stop();
                semaphore.Set();
            });

            stopwatch.Start();
            recorderService.Start();

            if (semaphore.WaitOne(WAIT_TIME_FOR_TIMER_ACTION))
            {
                Assert.That(stopwatch.ElapsedMilliseconds, Is.EqualTo(STAMPING_INTERVAL).Within(20));
            }
            else
            {
                Assert.Fail("Internal timer did not fired.");
            }
        }

        [Test]
        public void InternalTimer_raises_Stamping_event()
        {
            RecorderService recorderService = CreateRecorderService();
            ManualResetEvent semaphore = new ManualResetEvent(false);

            recorderService.Stamping += (sender, args) => semaphore.Set();
            recorderService.StampingInterval = TimeSpan.FromMilliseconds(STAMPING_INTERVAL);

            recorderService.Start();

            if (!semaphore.WaitOne(200))
            {
                Assert.Fail("Stamping event was not raised.");
            }
        }

        [Test]
        public void InternalTimer_raises_Stamped_event()
        {
            RecorderService recorderService = CreateRecorderService();
            ManualResetEvent semaphore = new ManualResetEvent(false);

            recorderService.Stamped += (sender, args) => semaphore.Set();
            recorderService.StampingInterval = TimeSpan.FromMilliseconds(STAMPING_INTERVAL);

            recorderService.Start();

            if (!semaphore.WaitOne(WAIT_TIME_FOR_TIMER_ACTION))
            {
                Assert.Fail("Stamped event was not raised.");
            }
        }
    }
}
