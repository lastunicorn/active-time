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
using DustInTheWind.ActiveTime.Common.Events;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Recording
{
    [TestFixture]
    public class RecorderServiceTests
    {
        private Mock<IScribe> scribMock;
        private Mock<IApplicationService> applicationService;

        /// <summary>
        /// Time interval in miliseconds that is used in tests for the StampingInterval of the RecorderService.
        /// This value is used by the RecorderService to configure its internal timer.
        /// </summary>
        private const int STAMPING_INTERVAL = 100;

        /// <summary>
        /// Time in miliseconds that is used in tests to wait for the internal timer to do its job.
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

        #region Constructor tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_scrib_is_null()
        {
            new RecorderService(null, applicationService.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_applicationService_is_null()
        {
            new RecorderService(scribMock.Object, null);
        }

        [Test]
        public void Constructor_successfully_called()
        {
            CreateRecorderService();
        }

        #endregion

        #region Start tests

        [Test]
        public void Start_stamps_a_new_record()
        {
            RecorderService recorderService = CreateRecorderService();
            scribMock.Setup(x => x.StampNew());

            recorderService.Start();

            scribMock.VerifyAll();
        }

        [Test]
        public void Start_changes_state_to_Running()
        {
            RecorderService recorderService = CreateRecorderService();

            recorderService.Start();

            Assert.That(recorderService.State, Is.EqualTo(RecorderState.Running));
        }

        [Test]
        public void Start_raises_Started_event()
        {
            RecorderService recorderService = CreateRecorderService();
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
            RecorderService recorderService = CreateRecorderService();
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
            RecorderService recorderService = CreateRecorderService();
            ManualResetEvent semaphore = new ManualResetEvent(false);
            recorderService.Start();
            recorderService.Started += (sender, args) => semaphore.Set();

            recorderService.Start();

            if (semaphore.WaitOne(WAIT_TIME_FOR_TIMER_ACTION))
            {
                Assert.Fail("Started event was raised. It was expected it would not.");
            }
        }

        #endregion

        #region StampingInterval tests

        [Test]
        public void StampingInterval_initial_value_is_1min()
        {
            RecorderService recorderService = CreateRecorderService();

            Assert.That(recorderService.StampingInterval, Is.EqualTo(TimeSpan.FromMinutes(1)));
        }

        [Test]
        public void StampingInterval_sets_correct_value()
        {
            RecorderService recorderService = CreateRecorderService();
            recorderService.StampingInterval = TimeSpan.FromSeconds(10);

            Assert.That(recorderService.StampingInterval, Is.EqualTo(TimeSpan.FromSeconds(10)));
        }

        #endregion

        #region Internal timer tests

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

        #endregion

        #region Stop tests

        [Test]
        public void Stop_raises_Stopped_event()
        {
            RecorderService recorderService = CreateRecorderService();
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
        public void Stop_does_not_raise_Stopped_event_if_recorder_is_already_stopped()
        {
            RecorderService recorderService = CreateRecorderService();
            ManualResetEvent semaphore = new ManualResetEvent(false);

            recorderService.Stopped += (sender, args) => semaphore.Set();
            recorderService.Stop();

            if (semaphore.WaitOne(WAIT_TIME_FOR_TIMER_ACTION))
            {
                Assert.Fail("Stopped event was raised. Was expected it would not.");
            }
        }

        #endregion

        #region GetTimeFromLastStop

        [Test]
        public void GetTimeFromLastStop_returns_null_if_not_started()
        {
            RecorderService recorderService = CreateRecorderService();

            Assert.That(recorderService.GetTimeFromLastStop(), Is.Null);
        }

        [Test]
        public void GetTimeFromLastStop_returns_null_if_started_but_never_stopped()
        {
            RecorderService recorderService = CreateRecorderService();
            recorderService.Start();

            Assert.That(recorderService.GetTimeFromLastStop(), Is.Null);
        }

        [Test]
        public void GetTimeFromLastStop_returns_correct_value_after_service_is_started_and_stopped()
        {
            RecorderService recorderService = CreateRecorderService();
            recorderService.Start();
            Thread.Sleep(50);
            recorderService.Stop();
            Thread.Sleep(100);

            Assert.That(recorderService.GetTimeFromLastStop(), Is.EqualTo(TimeSpan.FromMilliseconds(100)).Within(TimeSpan.FromMilliseconds(20)));
        }

        [Test]
        public void GetTimeFromLastStop_returns_correct_value_after_service_started_second_time()
        {
            RecorderService recorderService = CreateRecorderService();
            recorderService.Start();
            Thread.Sleep(50);
            recorderService.Stop();
            Thread.Sleep(50);
            recorderService.Start();
            Thread.Sleep(50);

            Assert.That(recorderService.GetTimeFromLastStop(), Is.EqualTo(TimeSpan.FromMilliseconds(100)).Within(TimeSpan.FromMilliseconds(20)));
        }

        #endregion

        [Test]
        public void Service_is_stopped_when_ApplicationService_raises_Exiting_event()
        {
            RecorderService recorderService = CreateRecorderService();
            recorderService.Start();

            applicationService.Raise(x=>x.Exiting += null, EventArgs.Empty);

            Assert.That(recorderService.State, Is.EqualTo(RecorderState.Stopped));
        }
    }
}
