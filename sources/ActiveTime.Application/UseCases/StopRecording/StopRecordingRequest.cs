using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.StopRecording
{
    public class StopRecordingRequest : IRequest
    {
        public bool DeleteLastRecord { get; set; }
    }
}