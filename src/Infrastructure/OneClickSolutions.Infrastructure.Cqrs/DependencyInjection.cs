﻿using System;
using OneClickSolutions.Infrastructure.Cqrs.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace OneClickSolutions.Infrastructure.Cqrs
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services,
            params Type[] handlerAssemblyMakerTypes)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddMediatR(handlerAssemblyMakerTypes);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            return services;
        }
    }
}