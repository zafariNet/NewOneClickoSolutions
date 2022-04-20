using System.Threading;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Dependency;

namespace OneClickSolutions.Infrastructure.Domain
{
    public interface IDomainEventHandler<in T> : ITransientDependency
        where T : IDomainEvent
    {
        Task Handle(T domainEvent, CancellationToken cancellationToken = default);
    }
}