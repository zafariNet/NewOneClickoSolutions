using OneClickSolutions.Domain.DocumentTypes.Catalogs;
using OneClickSolutions.Infrastructure.Domain;


public class DocumentTypeCreatedEventHandler : IDomainEventHandler<DocumentTypeCreated>
{
    public Task Handle(DocumentTypeCreated domainEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(domainEvent.DocumentName);
        return Task.CompletedTask;
    }
}
