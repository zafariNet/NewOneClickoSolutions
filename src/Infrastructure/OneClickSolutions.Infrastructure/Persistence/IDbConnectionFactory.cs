using System;
using System.Data;
using OneClickSolutions.Infrastructure.Dependency;

namespace OneClickSolutions.Infrastructure.Persistence
{
    public interface IDbConnectionFactory : IDisposable, IScopedDependency
    {
        IDbConnection Create();
    }
}