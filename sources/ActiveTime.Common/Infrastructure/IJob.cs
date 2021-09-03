namespace DustInTheWind.ActiveTime.Common.Infrastructure
{
    public interface IJob
    {
        string Id { get; }

        JobState State { get; }

        void Start();

        void Stop();
    }
}