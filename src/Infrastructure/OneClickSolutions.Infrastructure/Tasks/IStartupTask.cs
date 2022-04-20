using System.Threading;
using System.Threading.Tasks;

namespace OneClickSolutions.Infrastructure.Tasks
{
    public interface IStartupTask : ITask
    {
        Task Run(CancellationToken cancellationToken = default);
    }
}