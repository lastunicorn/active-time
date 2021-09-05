using DustInTheWind.ActiveTime.Common.Jobs;

namespace DustInTheWind.ActiveTime.Application.UseCases.PresentTray
{
    public class PresentTrayResponse
    {
        public JobState RecorderState { get; set; }
    }
}