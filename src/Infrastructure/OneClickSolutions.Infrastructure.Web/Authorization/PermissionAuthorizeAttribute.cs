using System;
using OneClickSolutions.Infrastructure.Authorization;
using OneClickSolutions.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace OneClickSolutions.Infrastructure.Web.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="AuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize</param>
        public PermissionAuthorizeAttribute(params string[] permissions)
        {
            Policy = $"{PermissionConstant.PolicyPrefix}{permissions.PackToString(PermissionConstant.PolicyNameSplitSymbol)}";
        }
    }
}