using OneClickSolutions.Infrastructure.EntityFrameworkCore.Application;

namespace OneClickSolutions.Domain.DocumentTypes.Catalogs
{
    public interface IDocumentTypePolicy:IDomainPolicy<DocumentType>
    {
        bool DocumentTypeNameMustBeUnique(DocumentType documentType);
    }
}
