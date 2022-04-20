using Microsoft.EntityFrameworkCore;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Hooks
{
    public abstract class PreFetchHook<TEntity> : PreActionHook<TEntity>
    {
        public override EntityState HookState => EntityState.Unchanged;
    }
}