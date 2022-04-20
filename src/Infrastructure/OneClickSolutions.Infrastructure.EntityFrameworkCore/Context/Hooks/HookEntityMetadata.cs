using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Hooks
{
    public class HookEntityMetadata
    {
        public HookEntityMetadata(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
    }
}