using System;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Common;
using OneClickSolutions.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace OneClickSolutions.Infrastructure.Web.Tenancy.Internal
{
    public class TenantPipelineMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApplicationBuilder _rootApp;
        private readonly Action<Tenant, IApplicationBuilder> _configuration;

        private readonly LockingConcurrentDictionary<Tenant, RequestDelegate> _pipelines
            = new LockingConcurrentDictionary<Tenant, RequestDelegate>();

        public TenantPipelineMiddleware(
            RequestDelegate next,
            IApplicationBuilder rootApp,
            Action<Tenant, IApplicationBuilder> configuration)
        {
            _next = next;
            _rootApp = rootApp;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var tenant = context.Tenant();
            if (tenant == null)
                throw new InvalidOperationException(
                    "TenantResolutionMiddleware must be register before TenantPipelineMiddleware");

            var tenantPipeline = _pipelines.GetOrAdd(tenant, BuildTenantPipeline);

            await tenantPipeline(context);
        }

        private RequestDelegate BuildTenantPipeline(Tenant tenant)
        {
            var branchBuilder = _rootApp.New();

            _configuration(tenant, branchBuilder);

            // register root pipeline at the end of the tenant branch
            branchBuilder.Run(_next);

            return branchBuilder.Build();
        }
    }
}