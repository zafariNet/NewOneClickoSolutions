using System;
using OneClickSolutions.Infrastructure.Timing;

namespace OneClickSolutions.Infrastructure.Domain
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime DateTime { get; }
    }

    public abstract class DomainEvent : IDomainEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime DateTime { get; } = SystemTime.Now();
    }
}