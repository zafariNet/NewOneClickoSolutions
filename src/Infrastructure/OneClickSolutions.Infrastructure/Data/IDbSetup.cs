using OneClickSolutions.Infrastructure.Dependency;

namespace OneClickSolutions.Infrastructure.Data
{
    public interface IDbSetup : ITransientDependency
    {
        void Seed();
    }
}