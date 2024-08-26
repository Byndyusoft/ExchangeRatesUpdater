using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Infrastructure.Services;

public static class DomainCommonServiceCollectionExtensions
{
    public static IServiceCollection AddDomainCommonServices(
        this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(DomainCommonServiceCollectionExtensions).Assembly)
            .AddClasses(classes => classes.WithAttribute<ServiceDescriptorAttribute>())
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());

        return services;
    }
}