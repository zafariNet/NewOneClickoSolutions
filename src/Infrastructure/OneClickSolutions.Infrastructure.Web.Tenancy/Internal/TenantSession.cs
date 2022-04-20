using System.Collections.Generic;
using System.Security.Claims;
using OneClickSolutions.Infrastructure.Extensions;
using OneClickSolutions.Infrastructure.GuardToolkit;
using OneClickSolutions.Infrastructure.Runtime;
using OneClickSolutions.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Http;

namespace OneClickSolutions.Infrastructure.Web.Tenancy.Internal
{
    internal sealed class TenantSession : ITenantSession
    {
        private readonly IHttpContextAccessor _context;
        private readonly IUserSession _session;

        public TenantSession(IHttpContextAccessor context, IUserSession session)
        {
            _session = Ensure.IsNotNull(session, nameof(session));
            _context = Ensure.IsNotNull(context, nameof(context));
        }

        private ClaimsPrincipal Principal => _context?.HttpContext?.User;
        private Tenant Tenant => _context?.HttpContext?.Tenant();
        public string TenantId => _session.IsAuthenticated ? Principal?.FindTenantId() : Tenant?.Id;
        public string TenantName => _session.IsAuthenticated ? Principal?.FindTenantName() : Tenant?.Name;
        public bool IsHeadTenant => Principal?.IsHeadTenant() ?? false;
        public string ImpersonatorTenantId => Principal?.FindImpersonatorTenantId();
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();
    }
}