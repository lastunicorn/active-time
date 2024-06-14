using DustInTheWind.ActiveTime.Application.Recording2;

namespace DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.PresentTimeRecords
{
    public class PresentTimeRecordsResponse
    {
        public DayTimeInterval[] Records { get; set; }
    }
}