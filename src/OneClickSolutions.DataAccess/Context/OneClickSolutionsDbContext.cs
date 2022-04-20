using System.Collections.Generic;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Caching;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Converters.Json;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Extensions;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Hooks;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Cryptography;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Logging;

namespace OneClickSolutions.DataAccess.Context
{
    public class OneClickSolutionsDbContext : DbContextCore
    {
        public OneClickSolutionsDbContext(
            DbContextOptions<OneClickSolutionsDbContext> options,
            IEnumerable<IHook> hooks) : base(options, hooks)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyLogConfiguration();
            modelBuilder.ApplyProtectionKeyConfiguration();
            modelBuilder.ApplySqlCacheConfiguration();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.AddJsonFields();
            modelBuilder.AddTrackingFields<long>();
            modelBuilder.AddTenancyField<long>();
            modelBuilder.AddIsDeletedField();
            modelBuilder.AddRowVersionField();
            modelBuilder.AddRowIntegrityField();
            modelBuilder.AddRowLevelSecurityField<long>();

            modelBuilder.NormalizeDateTime();
            modelBuilder.NormalizeDecimal(20, 6);


            base.OnModelCreating(modelBuilder);
        }
    }
}