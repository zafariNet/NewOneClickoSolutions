using System;
using OneClickSolutions.Infrastructure.Cryptography;
using OneClickSolutions.Infrastructure.IO;
using OneClickSolutions.Infrastructure.Runtime;
using OneClickSolutions.Infrastructure.Web.Authorization;
using OneClickSolutions.Infrastructure.Web.Cryptography;
using OneClickSolutions.Infrastructure.Web.Hosting;
using OneClickSolutions.Infrastructure.Web.IO;
using OneClickSolutions.Infrastructure.Web.Runtime;
using OneClickSolutions.Infrastructure.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace OneClickSolutions.Infrastructure.Web
{
    public static class DependencyInjection
    {
        public static WebFrameworkBuilder AddWebFramework(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddHttpContextAccessor();
            services.AddScoped<IUserSession, UserSession>();
            
            return new WebFrameworkBuilder(services);
        }
    }

    /// <summary>
    /// Configure DNTFrameworkCore.Web services
    /// </summary>
    public class WebFrameworkBuilder
    {
        public IServiceCollection Services { get; }

        public WebFrameworkBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public WebFrameworkBuilder WithProtection()
        {
            Services.AddSingleton<IProtectionService, ProtectionService>();
            return this;
        }

        public WebFrameworkBuilder WithPermissionAuthorization()
        {
            Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            return this;
        }

        public WebFrameworkBuilder WithAntiXsrf()
        {
            Services.AddScoped<IAntiXsrf, AntiXsrf>();
            return this;
        }

        public WebFrameworkBuilder WithPasswordHashAlgorithm()
        {
            Services.AddSingleton<IUserPasswordHashAlgorithm, UserPasswordHashAlgorithm>();
            return this;
        }

        public WebFrameworkBuilder WithQueuedHostedService()
        {
            Services.AddHostedService<QueuedHostedService>();
            return this;
        }
        
        public WebFrameworkBuilder WithTaskHostedService()
        {
            Services.AddHostedService<TaskHostedService>();
            return this;
        }
        
        public WebFrameworkBuilder WithEnvironmentPath()
        {
            Services.AddSingleton<IEnvironmentPath, EnvironmentPath>();
            return this;
        }

        /// <summary>
        /// Adds IFileNameSanitizer to IServiceCollection.
        /// </summary>
        public WebFrameworkBuilder WithSafeFileSanitizer()
        {
            Services.AddTransient<IFileNameSanitizer, FileNameSanitizer>();
            return this;
        }
    }
}