using System;
using OneClickSolutions.Infrastructure.Dependency;

namespace OneClickSolutions.Infrastructure.Tenancy
{
    public interface ITenantContainerFactory : ISingletonDependency
    {
        IServiceProvider CreateContainer(string tenantId);
    }
}