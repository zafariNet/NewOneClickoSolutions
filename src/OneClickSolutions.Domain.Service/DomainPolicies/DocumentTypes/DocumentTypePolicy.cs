using OneClickSolutions.Domain.DocumentTypes;
using OneClickSolutions.Domain.DocumentTypes.Catalogs;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Application;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;

namespace OneClickSolutions.Domain.Service.DomainPolicies.DocumentTypes
{
    public class DocumentTypePolicy : DomainPolicy<DocumentType>, IDocumentTypePolicy
    {
        public DocumentTypePolicy(IDbContext dbContext) : base(dbContext)
        {
        }

        public bool DocumentTypeNameMustBeUnique(DocumentType documentType)
        {
            return EntitySet.Where(x => x.Name == documentType.Name).Any();
        }
    }
}
