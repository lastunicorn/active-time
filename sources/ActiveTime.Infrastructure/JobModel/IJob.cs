using System.Threading.Tasks;

namespace DustInTheWind.ActiveTime.Infrastructure.JobModel
{
    public interface IJob
    {
        string Id { get; }

        JobState State { get; }

        Task Start();

        Task Stop();
    }
}