using System;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Application;
using OneClickSolutions.Infrastructure.Configuration;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;
using OneClickSolutions.Infrastructure.Functional;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Configuration
{
    internal sealed class KeyValueService : ApplicationService, IKeyValueService
    {
        private readonly IDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly DbSet<KeyValue> _values;

        public KeyValueService(IDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _values = _dbContext.Set<KeyValue>();
        }

        public async Task SetValueAsync(string key, string value)
        {
            var record = await _values.FirstOrDefaultAsync(v => v.Key == key);
            if (record == null)
            {
                _values.Add(new KeyValue
                {
                    Key = key,
                    Value = value
                });
            }
            else
            {
                record.Value = value;
            }

            await _dbContext.SaveChangesAsync();

            ReloadConfiguration();
        }

        public async Task<Maybe<string>> LoadValueAsync(string key)
        {
            var keyValue = await _values.FirstOrDefaultAsync(v => v.Key == key);
            return keyValue == null ? Maybe<string>.None : keyValue.Value;
        }

        private void ReloadConfiguration()
        {
            ((IConfigurationRoot) _configuration).Reload();
        }
    }
}