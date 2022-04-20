using Microsoft.Extensions.DependencyInjection;
using OneClickSolutions.Infrastructure.Dependency;
using OneClickSolutions.Infrastructure.Eventing;

namespace OneClickSolutions.Domain.Service;
public static class DependencyInjection
{
    public static void AddDomainPolicies(this IServiceCollection services)
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