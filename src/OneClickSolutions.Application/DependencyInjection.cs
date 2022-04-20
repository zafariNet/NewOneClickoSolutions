using Castle.DynamicProxy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OneClickSolutions.Application.Localization;
using OneClickSolutions.Infrastructure.Application;
using OneClickSolutions.Infrastructure.Dependency;
using OneClickSolutions.Infrastructure.Domain;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Transaction;
using OneClickSolutions.Infrastructure.Eventing;
using OneClickSolutions.Infrastructure.FluentValidation;
using OneClickSolutions.Infrastructure.Validation;
using OneClickSolutions.Infrastructure.Validation.Interception;
using System.Reflection;

namespace OneClickSolutions.Application;
public static class DependencyInjection
{
    private static readonly ProxyGenerator ProxyGenerator = new();

    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection));
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient<ITranslationService, NullTranslationService>();
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
            .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IModelValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        // scan Domain Event handlers for resolving implementation
        services.Scan(scan => scan
            .FromAssemblyOf<EventAndBusinessHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                  .AsSelfWithInterfaces()
            .WithTransientLifetime());

        foreach (var descriptor in services.Where(s => typeof(IApplicationService).IsAssignableFrom(s.ServiceType))
            .ToList())
        {
            services.Decorate(descriptor.ServiceType, (target, serviceProvider) =>
                ProxyGenerator.CreateInterfaceProxyWithTargetInterface(
                    descriptor.ServiceType,
                    target, serviceProvider.GetRequiredService<ValidationInterceptor>(),
                    serviceProvider.GetRequiredService<TransactionInterceptor>()));
        }
        services.ConfigureMappingProfilesWithDependency();
    }

}