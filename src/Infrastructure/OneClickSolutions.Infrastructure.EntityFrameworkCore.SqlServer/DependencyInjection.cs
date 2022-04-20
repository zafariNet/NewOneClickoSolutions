using System;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Hooks;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.SqlServer.Numbering;
using OneClickSolutions.Infrastructure.Numbering;
using Microsoft.Extensions.DependencyInjection;

namespace  OneClickSolutions.Infrastructure.EntityFrameworkCore.SqlServer
{
    public static class DependencyInjection
    {
        public static EFCoreBuilder WithNumberingHook(this EFCoreBuilder builder, Action<NumberingOptions> options)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (options == null) throw new ArgumentNullException(nameof(options));

            builder.Services.Configure(options);
            builder.Services.AddScoped<IHook, PreInsertNumberedEntityHook>();

            return builder;
        }
    }
}