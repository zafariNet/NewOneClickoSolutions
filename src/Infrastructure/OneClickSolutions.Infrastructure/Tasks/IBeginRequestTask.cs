using System.Threading;
using System.Threading.Tasks;

namespace OneClickSolutions.Infrastructure.Tasks
{
    public interface IBeginRequestTask : ITask
    {
        Task Run(CancellationToken cancellationToken = default);
    }
}