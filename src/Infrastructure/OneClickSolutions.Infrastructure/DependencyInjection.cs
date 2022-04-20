using System;
using OneClickSolutions.Infrastructure.Caching;
using OneClickSolutions.Infrastructure.Cryptography;
using OneClickSolutions.Infrastructure.Dependency;
using OneClickSolutions.Infrastructure.Eventing;
using OneClickSolutions.Infrastructure.Tasks;
using OneClickSolutions.Infrastructure.Threading.BackgroundTasks;
using OneClickSolutions.Infrastructure.Timing;
using OneClickSolutions.Infrastructure.Validation;
using OneClickSolutions.Infrastructure.Validation.Interception;
using Microsoft.Extensions.DependencyInjection;

namespace OneClickSolutions.Infrastructure
{
    public static class DependencyInjection
    {
        // ReSharper disable once InconsistentNaming
        public static FrameworkBuilder AddFramework(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IEventBus, EventBus>();
            services.AddSingleton<ISystemClock, SystemClock>();
            services.AddTransient(typeof(Lazy<>), typeof(LazyFactory<>));

            return new FrameworkBuilder(services);
        }

        public static IServiceCollection AddStartupTask<TStartupTask>(this IServiceCollection services)
            where TStartupTask : class, IStartupTask
        {
            return services.AddScoped<IStartupTask, TStartupTask>();
        }
    }

    /// <summary>
    /// Configure OneClickSolutions.Infrastructure services
    /// </summary>
    public sealed class FrameworkBuilder
    {
        public IServiceCollection Services { get; }

        public FrameworkBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Register the ISecurityService
        /// </summary>
        public FrameworkBuilder WithSecurityService()
        {
            Services.AddSingleton<ISecurityService, SecurityService>();
            return this;
        }

        /// <summary>
        /// Register the ICacheService
        /// </summary>
        public FrameworkBuilder WithMemoryCache()
        {
            Services.AddMemoryCache();
            Services.AddSingleton<ICacheService, MemoryCacheService>();
            return this;
        }

        /// <summary>
        /// Register the IBackgroundTaskQueue
        /// </summary>
        public FrameworkBuilder WithBackgroundTaskQueue()
        {
            Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            return this;
        }

        /// <summary>
        /// Register the IRandomNumberProvider
        /// </summary>
        public FrameworkBuilder WithRandomNumber()
        {
            Services.AddSingleton<IRandomNumber, RandomNumber>();
            return this;
        }

        /// <summary>
        /// Register the ITaskEngine
        /// </summary>
        public FrameworkBuilder WithTaskEngine()
        {
            Services.AddScoped<ITaskEngine, TaskEngine>();
            return this;
        }

        /// <summary>
        /// Register the validation infrastructure's services
        /// </summary>
        public FrameworkBuilder WithModelValidation(Action<ValidationOptions> setupAction = null)
        {
            Services.AddTransient<ValidationInterceptor>();
            Services.AddTransient<MethodInvocationValidator>();
            Services.AddTransient<IMethodParameterValidator, DataAnnotationMethodParameterValidator>();
            Services.AddTransient<IMethodParameterValidator, ValidatableObjectMethodParameterValidator>();
            Services.AddTransient<IMethodParameterValidator, ModelValidationMethodParameterValidator>();

            if (setupAction != null)
            {
                Services.Configure(setupAction);
            }

            return this;
        }
    }
}