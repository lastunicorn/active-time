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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Events;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Microsoft.Practices.Prism.Events;
using Moq;
using NUnit.Framework;
using System.Threading;

namespace DustInTheWind.ActiveTime.UnitTests.Recording
{
    [TestFixture]
    public class RecorderServiceTests
    {
        private Mock<IScrib> scribMock;
        private Mock<IEventAggregator> eventAggregatorMock;

        [SetUp]
        public void SetUp()
        {
            scribMock = new Mock<IScrib>();
            eventAggregatorMock = new Mock<IEventAggregator>();
        }

        private RecorderService CreateRecorderService()
        {
            return new RecorderService(scribMock.Object, eventAggregatorMock.Object);
        }

        #region Constructor Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_scrib_is_null()
        {
            new RecorderService(null, eventAggregatorMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_eventAggregator_is_null()
        {
            new RecorderService(scribMock.Object, null);
        }

        [Test]
        public void Constructor_successfully_called()
        {
            CreateRecorderService();
        }

        [Test]
        public void Constructor_subscribes_to_ApplicationExitEvent()
        {
            ApplicationExitEvent ev = new ApplicationExitEvent();
            eventAggregatorMock.Setup(x => x.GetEvent<ApplicationExitEvent>()).Returns(ev);

            RecorderService recorderService = CreateRecorderService();

            eventAggregatorMock.VerifyAll();
        }

        #endregion

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
            bool eventCalled = false;
            recorderService.Started += (s, e) =>
                                           {
                                               eventCalled = true;
                                           };

            recorderService.Start();

            Assert.That(eventCalled, Is.True);
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

        [Test]
        public void InternalTimer_stamps_at_correct_time_interval()
        {
            Stopwatch stopwatch = new Stopwatch();
            ManualResetEvent semaphore = new ManualResetEvent(false);

            RecorderService recorderService = CreateRecorderService();
            recorderService.StampingInterval = TimeSpan.FromMilliseconds(100);
            scribMock.Setup(x => x.Stamp()).Callback(() =>
            {
                stopwatch.Stop();
                semaphore.Set();
            });

            stopwatch.Start();
            recorderService.Start();

            if (semaphore.WaitOne(200))
            {
                Assert.That(stopwatch.ElapsedMilliseconds, Is.EqualTo(100).Within(50));
            }
            else
            {
                Assert.Fail("Internal timer did not fired.");
            }
        }

        public void InternalTimer_raises_Stamping_event()
        {
            
        }
    }
}
