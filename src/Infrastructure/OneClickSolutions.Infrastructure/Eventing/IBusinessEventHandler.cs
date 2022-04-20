using System.Threading;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Dependency;
using OneClickSolutions.Infrastructure.Functional;

namespace OneClickSolutions.Infrastructure.Eventing
{
    public interface IBusinessEventHandler<in T> : ITransientDependency
        where T : IBusinessEvent
    {
        Task<Result> Handle(T businessEvent, CancellationToken cancellationToken = default);
    }
}