using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneClickSolutions.DataAccess.Context;
using OneClickSolutions.Infrastructure.Dependency;
using OneClickSolutions.Infrastructure.EntityFrameworkCore;
using OneClickSolutions.Infrastructure.Eventing;

namespace OnclickSolutions.AutoClick.DataAccess
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEFCore<OneClickSolutionsDbContext>()
                .WithTrackingHook<long>()
                .WithDeletedEntityHook()
                .WithRowLevelSecurityHook<long>()
                .WithRowIntegrityHook();

            services.AddDbContext<OneClickSolutionsDbContext>((provider, builder) =>
            {
                builder.AddInterceptors(provider.GetRequiredService<SecondLevelCacheInterceptor>());
                builder.EnableSensitiveDataLogging();
                builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        optionsBuilder =>
                        {
                            var seconds = (int) TimeSpan
                                .FromMinutes(configuration.GetValue(nameof(optionsBuilder.CommandTimeout),
                                    3)).TotalSeconds;
                            optionsBuilder.CommandTimeout(seconds);
                            optionsBuilder.MigrationsHistoryTable("MigrationHistory", "dbo");
                        })
                    .ConfigureWarnings(warnings =>
                    {
                        //...
                    });
            });
            AddInternalServices(services);

            //TODO: See https://github.com/VahidN/EFCoreSecondLevelCacheInterceptor
            services.AddEFSecondLevelCache(options =>
                options.UseMemoryCacheProvider(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                    .DisableLogging(true));
        }
        private static void AddInternalServices(IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo<ISingletonDependency>())
                .AsMatchingInterface()
                .WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo<IScopedDependency>())
                .AsMatchingInterface()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<ITransientDependency>())
                .AsMatchingInterface()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IBusinessEventHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }
    }
}