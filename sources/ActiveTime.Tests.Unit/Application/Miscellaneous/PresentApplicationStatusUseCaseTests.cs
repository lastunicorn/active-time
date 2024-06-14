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
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Application.Miscellaneous.PresentApplicationStatus;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Application.Miscellaneous;

[TestFixture]
public class PresentApplicationStatusUseCaseTests
{
    private Mock<IJob> recorderJob;
    private StatusInfoService statusInfoService;
    private PresentApplicationStatusUseCase useCase;

    [SetUp]
    public void SetUp()
    {
        EventBus eventBus = new();
        statusInfoService = new StatusInfoService(eventBus);

        recorderJob = new Mock<IJob>();
        recorderJob
            .SetupGet(x => x.Id)
            .Returns(JobNames.Recorder);

        ScheduledJobs scheduledJobs = new();
        scheduledJobs.Add(recorderJob.Object);

        useCase = new PresentApplicationStatusUseCase(statusInfoService, scheduledJobs);
    }

    [Test]
    public async Task HavingRunningJob_WhenUseCaseIsExecuted_ThanIsRecorderStartedIsTrue()
    {
        recorderJob
            .SetupGet(x => x.State)
            .Returns(JobState.Running);

        PresentApplicationStatusResponse response = await ExecuteUseCase();

        response.IsRecorderStarted.Should().BeTrue();
    }

    [Test]
    public async Task HavingStoppedJob_WhenUseCaseIsExecuted_ThanIsRecorderStartedIsFalse()
    {
        recorderJob
            .SetupGet(x => x.State)
            .Returns(JobState.Stopped);

        PresentApplicationStatusResponse response = await ExecuteUseCase();

        response.IsRecorderStarted.Should().BeFalse();
    }

    [Test]
    public async Task HavingAStatusTextInStatusInfoService_WhenUseCaseIsExecuted_ThanResponseContainsTheStatusText()
    {
        statusInfoService.SetStatus("some status text");

        PresentApplicationStatusResponse response = await ExecuteUseCase();

        response.StatusText.Should().Be("some status text");
    }

    private async Task<PresentApplicationStatusResponse> ExecuteUseCase()
    {
        PresentApplicationStatusRequest request = new();
        return await useCase.Handle(request, CancellationToken.None);
    }
}