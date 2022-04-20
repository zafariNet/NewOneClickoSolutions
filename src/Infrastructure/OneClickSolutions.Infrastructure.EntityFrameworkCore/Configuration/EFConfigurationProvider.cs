using System;
using System.Linq;
using OneClickSolutions.Infrastructure.Configuration;
using OneClickSolutions.Infrastructure.Dependency;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Configuration
{
    public class EFConfigurationProvider : ConfigurationProvider
    {
        private readonly IServiceProvider _provider;

        public EFConfigurationProvider(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public override void Load()
        {
            _provider.RunScoped<IDbContext>(dbContext =>
            {
                Data?.Clear();
                Data = dbContext.Set<KeyValue>()
                    .AsNoTracking()
                    .ToDictionary(c => c.Key, c => c.Value);
            });
        }
    }
}