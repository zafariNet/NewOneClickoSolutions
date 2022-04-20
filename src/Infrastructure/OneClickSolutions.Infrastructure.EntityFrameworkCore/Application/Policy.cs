using Microsoft.EntityFrameworkCore;
using OneClickSolutions.Infrastructure.Dependency;
using OneClickSolutions.Infrastructure.Domain;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Application
{
    public class DomainPolicy<TEntity> : IDomainPolicy<TEntity>, ITransientDependency where TEntity : class, IEntity
    {

        public DomainPolicy(IDbContext dbContext)
        {
            this.EntitySet = dbContext.Set<TEntity>();
        }

        public DbSet<TEntity> EntitySet { get; }
    }
}
