using OneClickSolutions.Domain.DocumentTypes;
using OneClickSolutions.Domain.DocumentTypes.Repositories;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Persistence;
namespace OneClickSolutions.DataAccess.Repositories.DocumentTypes;
public class DocumentTypeRepository : RepositoryBase<DocumentType, Guid>, IDocumentTypeRepository
{
    public DocumentTypeRepository(IDbContext dbContext) : base(dbContext)
    {

    }
}
