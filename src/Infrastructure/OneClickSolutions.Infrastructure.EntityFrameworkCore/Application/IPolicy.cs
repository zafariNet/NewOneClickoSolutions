using Microsoft.EntityFrameworkCore;
using OneClickSolutions.Infrastructure.Domain;
namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Application
{
    public interface IDomainPolicy<TEntity> where TEntity : class,IEntity
    {
        DbSet<TEntity> EntitySet { get; }
    }
}
