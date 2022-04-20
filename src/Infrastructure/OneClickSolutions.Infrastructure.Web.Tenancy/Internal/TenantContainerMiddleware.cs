using System;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace OneClickSolutions.Infrastructure.Web.Tenancy.Internal
{
    //Under development
    internal sealed class TenantContainerMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantContainerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITenantContainerFactory factory)
        {
            var tenant = context.Tenant();
            if (tenant == null)
                throw new InvalidOperationException(
                    "TenantResolutionMiddleware must be register before TenantContainerMiddleware");

            using (var scope = factory.CreateContainer(tenant.Id).CreateScope())
            {
                context.RequestServices = scope.ServiceProvider;
                await _next.Invoke(context);
            }
        }
    }
}