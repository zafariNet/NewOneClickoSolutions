using System;
using OneClickSolutions.Infrastructure.Tenancy;
using OneClickSolutions.Infrastructure.Web.Tenancy.Internal;
using Microsoft.AspNetCore.Builder;

namespace OneClickSolutions.Infrastructure.Web.Tenancy
{
    /// <summary>
    ///     Nice method to register our middleware
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        ///     Use the Tenant Middleware to process the request
        /// </summary>
        public static IApplicationBuilder UseTenancy(this IApplicationBuilder builder) =>
            builder.UseMiddleware<TenantResolutionMiddleware>();

//        /// <summary>
//        ///     Use the Tenant Container Middleware to TenantScoped Dependency Injection
//        /// </summary>
//        public static IApplicationBuilder UseTenantContainer(this IApplicationBuilder builder) =>
//            builder.UseMiddleware<TenantContainerMiddleware>();

        /// <summary>
        /// Forking the pipeline - adding tenant specific middlewares 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePerTenant(this IApplicationBuilder app,
            Action<Tenant, IApplicationBuilder> configuration)
        {
            app.Use(next => new TenantPipelineMiddleware(next, app, configuration).Invoke);

            return app;
        }
    }
}