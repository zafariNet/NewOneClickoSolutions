using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Hooks;

namespace OneClickSolutions.DataAccess.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OneClickSolutionsDbContext>
    {
        public OneClickSolutionsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<OneClickSolutionsDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);

            return new OneClickSolutionsDbContext(builder.Options, Enumerable.Empty<IHook>());
        }
    }
}