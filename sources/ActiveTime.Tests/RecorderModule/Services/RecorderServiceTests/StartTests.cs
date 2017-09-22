// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using ActiveTime.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.RecorderModule.Services.RecorderServiceTests
{
    [TestFixture]
    public class StartTests
    {
        private Mock<IScribe> scribMock;
        private Mock<IApplicationService> applicationService;
        private RecorderService recorderService;

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
        public void stamps_a_new_record()
        {
            scribMock.Setup(x => x.StampNew());

            recorderService.Start();

            scribMock.VerifyAll();
        }

        [Test]
        public void Start_changes_state_to_Running()
        {

            recorderService.Start();

            Assert.That(recorderService.State, Is.EqualTo(RecorderState.Running));
        }

        [Test]
        public void Start_raises_Started_event()
        {
            bool eventRaised = false;
            recorderService.Started += (s, e) =>
                                           {
                                               eventRaised = true;
                                           };

            recorderService.Start();

            Assert.That(eventRaised, Is.True);
        }

        [Test]
        public void Start_raises_Started_event_with_not_null_parameters()
        {
            object sender = null;
            EventArgs eventArgs = null;
            recorderService.Started += (s, e) =>
            {
                sender = s;
                eventArgs = e;
            };

            recorderService.Start();

            Assert.That(sender, Is.Not.Null);
            Assert.That(eventArgs, Is.Not.Null);
        }

        [Test]
        public void Start_does_not_raise_Started_event_if_recorder_is_already_started()
        {
            ManualResetEvent semaphore = new ManualResetEvent(false);
            recorderService.Start();
            recorderService.Started += (sender, args) => semaphore.Set();

            recorderService.Start();

            if (semaphore.WaitOne(WAIT_TIME_FOR_TIMER_ACTION))
            {
                Assert.Fail("Started event was raised. It was expected it would not.");
            }
        }
    }
}
