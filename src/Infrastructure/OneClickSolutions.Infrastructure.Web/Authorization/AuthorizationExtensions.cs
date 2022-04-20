using OneClickSolutions.Infrastructure.Runtime;
using Microsoft.AspNetCore.Authorization;

namespace OneClickSolutions.Infrastructure.Web.Authorization
{
    public static class AuthorizationExtensions
    {
        public static void AddHeadOfficeOnlyPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.HeadOfficeOnly,
                policy => policy.RequireClaim(UserClaimTypes.IsHeadOffice, "true"));
        }

        public static void AddHeadTenantOnlyPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.HeadTenantOnly,
                policy => policy.RequireClaim(UserClaimTypes.IsHeadTenant, "true"));
        }
    }
}