namespace DustInTheWind.ActiveTime.Common.Jobs
{
    public interface IJob
    {
        string Id { get; }

        JobState State { get; }

        void Start();

        void Stop();
    }
}