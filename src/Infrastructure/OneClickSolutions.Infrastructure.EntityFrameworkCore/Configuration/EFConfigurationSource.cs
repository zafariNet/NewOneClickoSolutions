using System;
using Microsoft.Extensions.Configuration;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEFCore(this IConfigurationBuilder builder,
            IServiceProvider provider)
        {
            return builder.Add(new EFConfigurationSource(provider));
        }
    }

    public class EFConfigurationSource : IConfigurationSource
    {
        private readonly IServiceProvider _provider;

        public EFConfigurationSource(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EFConfigurationProvider(_provider);
        }
    }
}