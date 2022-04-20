using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.SqlServer.Numbering
{
    public static class DbContextExtensions
    {
        public static void AcquireDistributedLock(this IDbContext context, string resource)
        {
            context.ExecuteSqlRawCommand(@"EXEC sp_getapplock @Resource={0}, @LockOwner={1}, 
                        @LockMode={2} , @LockTimeout={3};", resource, "Transaction", "Exclusive", 15000);
        }
    }
}