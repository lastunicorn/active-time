﻿// ActiveTime
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

using System.Threading;
using DustInTheWind.ActiveTime.Application.Recording.Stamp;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Jobs;
using MediatR;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Jobs.RecorderJobTests
{
    [TestFixture]
    public class StartTests
    {
        private Mock<IMediator> mediator;
        private Mock<DustInTheWind.ActiveTime.Infrastructure.ITimer> timer;
        private RecorderJob recorderJob;

        [SetUp]
        public void SetUp()
        {
            mediator = new Mock<IMediator>();
            timer = new Mock<DustInTheWind.ActiveTime.Infrastructure.ITimer>();

            recorderJob = new RecorderJob(mediator.Object, timer.Object);
        }

        [Test]
        public void HavingRecorderJob_WhenStarted_ThenSendsAStampRequest()
        {
            recorderJob.Start();

            mediator.Verify(x => x.Send(It.IsAny<StampRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void HavingRecorderJob_WhenStarted_ThenStatusIsRunning()
        {
            recorderJob.Start();

            Assert.That(recorderJob.State, Is.EqualTo(JobState.Running));
        }
    }
}
