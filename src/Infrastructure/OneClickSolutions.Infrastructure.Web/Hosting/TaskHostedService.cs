using System.Threading;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Tasks;
using Microsoft.Extensions.Hosting;

namespace OneClickSolutions.Infrastructure.Web.Hosting
{
    internal sealed class TaskHostedService : IHostedService
    {
        private readonly ITaskEngine _engine;

        public TaskHostedService(ITaskEngine engine)
        {
            _engine = engine;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _engine.RunOnStartup(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _engine.RunOnEnd(cancellationToken);
        }
    }
}