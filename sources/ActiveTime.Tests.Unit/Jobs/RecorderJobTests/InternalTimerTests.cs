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
using DustInTheWind.ActiveTime.Application.UseCases.Stamp;
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
        private TimeAssertion timeAssertion;

        [SetUp]
        public void SetUp()
        {
            timeAssertion = new TimeAssertion();
            mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x.Send(It.IsAny<StampRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<MediatR.Unit>, CancellationToken>((request, cancellationToken) => timeAssertion.Signal());
        }

        [TearDown]
        public void TearDown()
        {
            timeAssertion.Dispose();
        }

        [Test]
        public void HavingRecorderJobIntervalConfiguredToStampAt100Millisecond_WhenStarted_ThenStampRequestIsSentAfter100Millisecond()
        {
            RecorderJob recorderJob = new RecorderJob(mediator.Object);
            recorderJob.StampingInterval = TimeSpan.FromMilliseconds(100);

            TimeSpan expected = TimeSpan.FromMilliseconds(100);
            timeAssertion.Run(expected, () =>
            {
                recorderJob.Start();
            });
        }
    }
}
