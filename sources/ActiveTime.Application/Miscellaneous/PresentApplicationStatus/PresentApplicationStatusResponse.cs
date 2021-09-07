using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.PresentApplicationStatus
{
    public class PresentApplicationStatusResponse : IRequest<PresentApplicationStatusResponse>
    {
        public bool IsRecorderStarted { get; set; }
        
        public string StatusText { get; set; }
    }
}