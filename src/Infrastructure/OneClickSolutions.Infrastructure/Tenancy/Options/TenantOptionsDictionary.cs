using System;
using OneClickSolutions.Infrastructure.Common;
using Microsoft.Extensions.Options;

namespace OneClickSolutions.Infrastructure.Tenancy.Options
{
    /// <summary>
    /// Dictionary of tenant specific options caches
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    internal sealed class
        TenantOptionsDictionary<TOptions> : LockingConcurrentDictionary<string, IOptionsMonitorCache<TOptions>>
        where TOptions : class
    {
        public TenantOptionsDictionary() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Get options for specific tenant (create if not exists)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public IOptionsMonitorCache<TOptions> Get(string tenantId)
        {
            return GetOrAdd(tenantId, key => new OptionsCache<TOptions>());
        }
    }
}