using System;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Authorization;
using OneClickSolutions.Infrastructure.Common;
using OneClickSolutions.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace OneClickSolutions.Infrastructure.Web.Authorization
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly LockingConcurrentDictionary<string, AuthorizationPolicy> _policies =
            new LockingConcurrentDictionary<string, AuthorizationPolicy>(StringComparer.OrdinalIgnoreCase);

        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PermissionConstant.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return await base.GetPolicyAsync(policyName);
            }

            var policy = _policies.GetOrAdd(policyName, static name =>
            {
                var permissions = name.Substring(PermissionConstant.PolicyPrefix.Length)
                    .UnpackFromString(PermissionConstant.PolicyNameSplitSymbol);

                return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new PermissionAuthorizationRequirement(permissions))
                    .Build();
            });

            return policy;
        }
    }
}