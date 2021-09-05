namespace DustInTheWind.ActiveTime.Infrastructure.JobModel
{
    public interface IJob
    {
        string Id { get; }

        JobState State { get; }

        void Start();

        void Stop();
    }
}