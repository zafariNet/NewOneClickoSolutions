using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Application;
using OneClickSolutions.Infrastructure.Functional;

namespace OneClickSolutions.Infrastructure.Configuration
{
    public interface IKeyValueService : IApplicationService
    {
        Task SetValueAsync(string key, string value);
        Task<Maybe<string>> LoadValueAsync(string key);
    }
}