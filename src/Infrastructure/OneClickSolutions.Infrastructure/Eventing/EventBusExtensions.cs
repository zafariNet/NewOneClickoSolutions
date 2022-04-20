using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Domain;

namespace OneClickSolutions.Infrastructure.Eventing
{
    public static class EventBusExtensions
    {
        public static Task Dispatch(this IEventBus bus, IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
        {
            var tasks = events.Select(async domainEvent => await bus.Dispatch(domainEvent, cancellationToken));
            return Task.WhenAll(tasks);
        }

        public static async Task DispatchDomainEvents(this IEventBus bus, IAggregateRoot aggregateRoot, CancellationToken cancellationToken = default)
        {
            var events = aggregateRoot.DomainEvents.ToArray();
            aggregateRoot.EmptyDomainEvents();
            await bus.Dispatch(events, cancellationToken);
        }
    }
}