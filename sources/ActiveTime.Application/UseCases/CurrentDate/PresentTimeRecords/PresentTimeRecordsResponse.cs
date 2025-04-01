using DustInTheWind.ActiveTime.Application.Recording;

namespace DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.PresentTimeRecords
{
    public class PresentTimeRecordsResponse
    {
        public DayTimeInterval[] Records { get; set; }
    }
}