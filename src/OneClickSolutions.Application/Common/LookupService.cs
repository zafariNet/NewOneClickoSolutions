using Microsoft.EntityFrameworkCore;
using OneClickSolutions.Domain.Identity;
using OneClickSolutions.Infrastructure.Common;
using OneClickSolutions.Infrastructure.Dependency;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;


namespace OneClickSolutions.Application.Common
{
    public interface ILookupService : IScopedDependency
    {
        Task<IReadOnlyList<LookupItem<long>>> FetchRolesAsync();
    }

    public class LookupService : ILookupService
    {
        private readonly IDbContext _dbContext;

        public LookupService(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyList<LookupItem<long>>> FetchRolesAsync()
        {
            var roles = await _dbContext.Set<Role>().AsNoTracking().Select(role => new LookupItem<long>
            {
                Text = role.Name,
                Value = role.Id
            }).ToListAsync();

            return roles;
        }
    }
}