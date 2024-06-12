using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Miscellaneous.PresentApplicationStatus;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Application.Miscellaneous
{
    [TestFixture]
    public class PresentApplicationStatusUseCaseTests
    {
        private Mock<IJob> recorderJob;
        private Mock<IStatusInfoService> statusInfoService;
        private PresentApplicationStatusUseCase useCase;

        [SetUp]
        public void SetUp()
        {
            statusInfoService = new Mock<IStatusInfoService>();

            recorderJob = new Mock<IJob>();
            recorderJob
                .SetupGet(x => x.Id)
                .Returns(JobNames.Recorder);

            ScheduledJobs scheduledJobs = new ScheduledJobs();
            scheduledJobs.Add(recorderJob.Object);

            useCase = new PresentApplicationStatusUseCase(statusInfoService.Object, scheduledJobs);
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
            statusInfoService
                .SetupGet(x => x.StatusText)
                .Returns("some status text");

            PresentApplicationStatusResponse response = await ExecuteUseCase();

            response.StatusText.Should().Be("some status text");
        }

        private async Task<PresentApplicationStatusResponse> ExecuteUseCase()
        {
            PresentApplicationStatusRequest request = new();
            return await useCase.Handle(request, CancellationToken.None);
        }
    }
}