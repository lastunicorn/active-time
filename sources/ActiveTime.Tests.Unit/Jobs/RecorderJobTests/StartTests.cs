// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.Stamp;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Jobs;
using Moq;
using NUnit.Framework;
using ITimer = DustInTheWind.ActiveTime.Infrastructure.ITimer;

namespace DustInTheWind.ActiveTime.Tests.Unit.Jobs.RecorderJobTests;

[TestFixture]
public class StartTests
{
    private Mock<IRequestBus> requestBus;
    private Mock<ITimer> timer;
    private RecorderJob recorderJob;

    [SetUp]
    public void SetUp()
    {
        requestBus = new Mock<IRequestBus>();
        timer = new Mock<ITimer>();

        recorderJob = new RecorderJob(requestBus.Object, timer.Object);
    }

    [Test]
    public async Task HavingRecorderJobConfiguredToRunAtStart_WhenStarted_ThenSendsAStampRequest()
    {
        recorderJob.RunOnStart = true;
        await recorderJob.Start();

        requestBus.Verify(x => x.Send(It.IsAny<StampRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task HavingRecorderJob_WhenStarted_ThenStatusIsRunning()
    {
        await recorderJob.Start();

        Assert.That(recorderJob.State, Is.EqualTo(JobState.Running));
    }
}