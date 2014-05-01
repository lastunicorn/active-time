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
using System.Threading;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.RecorderModule;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.RecorderModule.Services.RecorderServiceTests
{
    [TestFixture]
    public class StopTests
    {
        private Mock<IScribe> scribMock;
        private Mock<IApplicationService> applicationService;
        private RecorderService recorderService;

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

            recorderService = new RecorderService(scribMock.Object, applicationService.Object);
        }

        [Test]
        public void raises_Stopped_event()
        {
            ManualResetEvent semaphore = new ManualResetEvent(false);
            recorderService.Start();

            recorderService.Stopped += (sender, args) => semaphore.Set();
            recorderService.Stop();

            if (!semaphore.WaitOne(WAIT_TIME_FOR_TIMER_ACTION))
            {
                Assert.Fail("Stopped event was not raised.");
            }
        }

        [Test]
        public void does_not_raise_Stopped_event_if_recorder_is_already_stopped()
        {
            ManualResetEvent semaphore = new ManualResetEvent(false);

            recorderService.Stopped += (sender, args) => semaphore.Set();
            recorderService.Stop();

            if (semaphore.WaitOne(WAIT_TIME_FOR_TIMER_ACTION))
            {
                Assert.Fail("Stopped event was raised. Was expected it would not.");
            }
        }

        [Test]
        public void Service_is_stopped_when_ApplicationService_raises_Exiting_event()
        {
            recorderService.Start();

            applicationService.Raise(x=>x.Exiting += null, EventArgs.Empty);

            Assert.That(recorderService.State, Is.EqualTo(RecorderState.Stopped));
        }
    }
}
