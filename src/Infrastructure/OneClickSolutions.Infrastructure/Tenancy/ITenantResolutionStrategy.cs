namespace OneClickSolutions.Infrastructure.Tenancy
{
    public interface ITenantResolutionStrategy
    {
        string TenantId();
    }
}