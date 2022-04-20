using OneClickSolutions.Infrastructure.Domain;

namespace OneClickSolutions.Domain.DocumentTypes.Catalogs
{
    public class DocumentTypeCreated : IDomainEvent
    {
        public DocumentTypeCreated(string name)
        {
            DocumentName=name;
        }
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }
        public string DocumentName { get; protected set; }
    }
    public class DocumentTypeCreated0 : IDomainEvent
    {
        public DocumentTypeCreated0(string name)
        {
            DocumentName = name;
        }
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }
        public string DocumentName { get; protected set; }
    }
}