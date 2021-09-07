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
using System.Diagnostics;
using System.Threading;
using DustInTheWind.ActiveTime.Application.UseCases.Stamp;
using DustInTheWind.ActiveTime.Jobs;
using MediatR;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Jobs.RecorderJobTests
{
    [TestFixture]
    public class InternalTimerTests
    {
        private Mock<IMediator> mediator;
        private ManualResetEvent manualResetEvent;

        /// <summary>
        /// Time interval in milliseconds that is used in tests for the StampingInterval of the RecorderService.
        /// This value is used by the RecorderService to configure its internal timer.
        /// </summary>
        private const int StampingInterval = 100;

        /// <summary>
        /// Time in milliseconds that is used in tests to wait for the internal timer to do its job.
        /// </summary>
        private const int TimeoutMilliseconds = 200;

        [SetUp]
        public void SetUp()
        {
            mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x.Send(It.IsAny<StampRequest>(), It.IsAny<CancellationToken>()))
                .Callback<StampRequest>(x => manualResetEvent.Set());

            manualResetEvent = new ManualResetEvent(false);
        }

        [Test]
        public void InternalTimer_stamps_at_correct_time_interval()
        {
            RecorderJob recorderJob = new RecorderJob(mediator.Object);
            recorderJob.StampingInterval = TimeSpan.FromMilliseconds(StampingInterval);

            AssertTimeInterval(StampingInterval, "Job did not fired.", () =>
            {
                recorderJob.Start();
            });
        }

        private void AssertTimeInterval(int expectedMillis, string message, Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            action();
            manualResetEvent.WaitOne(TimeoutMilliseconds);

            long actualMillis = stopwatch.ElapsedMilliseconds;

            Assert.That(actualMillis, Is.EqualTo(expectedMillis).Within(20), () => message);
        }
    }
}
