using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OneClickSolutions.Application.DocumentTypes.Mappings;
using OneClickSolutions.Domain.DocumentTypes.Catalogs;

namespace OneClickSolutions.Application;
public static class MappingProfileExtensions
{
    public static void ConfigureMappingProfilesWithDependency(this IServiceCollection services)
    {
        services.AddTransient(provider => new MapperConfiguration(cfg =>
            {
                // Add profiles with dependency 
                cfg.AddProfile(new DocumentTypeMapper(provider.GetService<IDocumentTypePolicy>()));
            })
      .CreateMapper());
    }
}
