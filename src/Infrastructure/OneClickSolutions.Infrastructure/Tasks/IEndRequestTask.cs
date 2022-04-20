using System.Threading;
using System.Threading.Tasks;

namespace OneClickSolutions.Infrastructure.Tasks
{
    public interface IEndRequestTask : ITask
    {
        Task Run(CancellationToken cancellationToken = default);
    }
}