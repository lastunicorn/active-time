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
using System.Threading;
using DustInTheWind.ActiveTime.Application.Recording.Stamp;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Jobs;
using MediatR;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Jobs.RecorderJobTests
{
    [TestFixture]
    public class InternalTimerTests
    {
        private Mock<IMediator> mediator;
        private Mock<DustInTheWind.ActiveTime.Infrastructure.ITimer> timer;
        private TimeAssertion timeAssertion;

        [SetUp]
        public void SetUp()
        {
            timeAssertion = new TimeAssertion();

            mediator = new Mock<IMediator>();
            mediator
                .Setup(x => x.Send(It.IsAny<StampRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<MediatR.Unit>, CancellationToken>((request, cancellationToken) => timeAssertion.Signal());

            timer = new Mock<DustInTheWind.ActiveTime.Infrastructure.ITimer>();
        }

        [TearDown]
        public void TearDown()
        {
            timeAssertion.Dispose();
        }

        [Test]
        public void HavingRecorderJobRunning_WhenTimerIsTriggered_ThenStampRequestIsSentToMediator()
        {
            RecorderJob recorderJob = new RecorderJob(mediator.Object, timer.Object);
            recorderJob.Start();

            mediator.Invocations.Clear();

            timer.Raise(x => x.Tick += null, EventArgs.Empty);

            mediator.Verify(x => x.Send(It.IsAny<StampRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
