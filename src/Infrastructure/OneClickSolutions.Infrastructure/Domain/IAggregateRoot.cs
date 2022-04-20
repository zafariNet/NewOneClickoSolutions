namespace OneClickSolutions.Infrastructure.Domain
{
    public interface IAggregateRoot : IEntity
    {
    }

    public interface IAggregateRootVersion
    {
        int Version { get; }
        void IncrementVersion();
    }
}