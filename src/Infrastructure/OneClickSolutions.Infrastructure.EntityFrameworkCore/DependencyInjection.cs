using System;
using OneClickSolutions.Infrastructure.Configuration;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Configuration;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Hooks;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Transaction;
using OneClickSolutions.Infrastructure.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable InconsistentNaming

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore
{
    /// <summary>
    ///     Nice method to create the EFCore builder
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        ///     Add the services (application specific tenant class)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static EFCoreBuilder AddEFCore<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, IDbContext
        {
            services.AddScoped(provider => (IDbContext) provider.GetRequiredService(typeof(TDbContext)));
            services.AddScoped(provider => (IUnitOfWork) provider.GetRequiredService(typeof(TDbContext)));
            services.AddTransient<TransactionInterceptor>();
            services.AddScoped<IKeyValueService, KeyValueService>();
            services.AddScoped<IHook, PreUpdateRowVersionHook>();

            return new EFCoreBuilder(services, typeof(TDbContext));
        }
    }

    public class EFCoreBuilder
    {
        public EFCoreBuilder(IServiceCollection services, Type contextType)
        {
            Services = services;
            ContextType = contextType;
        }

        public IServiceCollection Services { get; }
        public Type ContextType { get; }

        public EFCoreBuilder WithTransactionOptions(Action<TransactionOptions> options)
        {
            Services.Configure(options);
            return this;
        }

        public EFCoreBuilder WithRowLevelSecurityHook<TUserId>() where TUserId : IEquatable<TUserId>
        {
            Services.AddScoped<IHook, PreInsertRowLevelSecurityHook<TUserId>>();
            return this;
        }

        public EFCoreBuilder WithTrackingHook<TUserId>() where TUserId : IEquatable<TUserId>
        {
            Services.AddScoped<IHook, PreInsertCreationTrackingHook<TUserId>>();
            Services.AddScoped<IHook, PreUpdateModificationTrackingHook<TUserId>>();
            return this;
        }

        public EFCoreBuilder WithTenancyHook<TTenantId>() where TTenantId : IEquatable<TTenantId>
        {
            Services.AddScoped<IHook, PreInsertTenantEntityHook<TTenantId>>();
            return this;
        }

        public EFCoreBuilder WithRowIntegrityHook()
        {
            Services.AddScoped<IHook, RowIntegrityHook>();
            return this;
        }

        public EFCoreBuilder WithDeletedEntityHook()
        {
            Services.AddScoped<IHook, PreDeleteDeletedEntityHook>();
            return this;
        }
    }
}