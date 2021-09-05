using DustInTheWind.ActiveTime.Infrastructure.JobModel;

namespace DustInTheWind.ActiveTime.Application.UseCases.PresentTray
{
    public class PresentTrayResponse
    {
        public JobState RecorderState { get; set; }
    }
}