using System;
using OneClickSolutions.Infrastructure.Common;
using OneClickSolutions.Infrastructure.Tenancy;
using OneClickSolutions.Infrastructure.Web.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace OneClickSolutions.Infrastructure.Web.Tenancy.Internal
{
    internal sealed class TenantContainerFactory : ITenantContainerFactory
    {
        private readonly LockingConcurrentDictionary<string, IServiceProvider> _providers =
            new LockingConcurrentDictionary<string, IServiceProvider>(StringComparer.OrdinalIgnoreCase);

        private readonly IServiceProvider _provider;
        private readonly IServiceCollection _services;

        public TenantContainerFactory(IServiceProvider provider, IServiceCollection services)
        {
            _provider = provider;
            _services = services;
        }

        public IServiceProvider CreateContainer(string tenantId)
        {
            return _providers.GetOrAdd(tenantId, key =>
            {
                var services = _provider.CreateChildContainer(_services);
                return services.BuildServiceProvider();
            });
        }
    }
}