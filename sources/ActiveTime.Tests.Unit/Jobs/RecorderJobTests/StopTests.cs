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

using DustInTheWind.ActiveTime.Infrastructure.JobEngine;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using DustInTheWind.ActiveTime.Jobs;
using Moq;
using NUnit.Framework;
using ITimer = DustInTheWind.ActiveTime.Infrastructure.JobEngine.ITimer;

namespace DustInTheWind.ActiveTime.Tests.Unit.Jobs.RecorderJobTests;

[TestFixture]
public class StopTests
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
    public void HavingARunningJob_WhenStopped_ThenStateIsStopped()
    {
        recorderJob.Start();

        recorderJob.Stop();

        Assert.That(recorderJob.State, Is.EqualTo(JobState.Stopped));
    }
}