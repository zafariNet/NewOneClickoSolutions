using OneClickSolutions.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Http;

namespace OneClickSolutions.Infrastructure.Web.Tenancy
{
    /// <summary>
    /// Extensions to HttpContext to make multi-tenancy easier to use
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Returns the current tenant
        /// </summary>
        public static Tenant Tenant(this HttpContext context)
        {
            if (!context.Items.ContainsKey(TenancyConstants.HttpContextItemName))
                return null;
            return context.Items[TenancyConstants.HttpContextItemName] as Tenant;
        }
    }
}