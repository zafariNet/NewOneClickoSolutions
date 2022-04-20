using System.Threading.Tasks;

namespace OneClickSolutions.Infrastructure.Tenancy
{
    public interface ITenantStore
    {
        Task<Tenant> FindTenantAsync(string tenantId);
    }
}