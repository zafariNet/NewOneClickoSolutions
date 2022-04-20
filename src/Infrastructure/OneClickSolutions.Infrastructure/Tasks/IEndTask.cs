using System.Threading;
using System.Threading.Tasks;

namespace OneClickSolutions.Infrastructure.Tasks
{
    public interface IEndTask : ITask
    {
        Task Run(CancellationToken cancellationToken = default);
    }
}