namespace DustInTheWind.ActiveTime.Common.ApplicationStatuses
{
    public class StampedStatus : ApplicationStatus
    {
        public override string Text { get; } = "Current record's time has been updated.";
    }
}