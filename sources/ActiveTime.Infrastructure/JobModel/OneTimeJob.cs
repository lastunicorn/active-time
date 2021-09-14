using System.Threading.Tasks;

namespace DustInTheWind.ActiveTime.Infrastructure.JobModel
{
    public abstract class OneTimeJob : JobBase
    {
        protected OneTimeJob(ITimer timer)
            : base(timer)
        {
        }

        protected override async Task OnExecuting()
        {
            await base.OnExecuting();
            await Stop();
        }
    }
}