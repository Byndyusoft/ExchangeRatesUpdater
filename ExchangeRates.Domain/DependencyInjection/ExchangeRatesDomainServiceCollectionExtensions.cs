// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

using Scrutor;

public static class ExchangeRatesDomainServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRatesDomain(
            this IServiceCollection services)
    {
        services.Scan(scan => scan
                              .FromAssemblies(typeof(ExchangeRatesDomainServiceCollectionExtensions).Assembly)
                              .AddClasses(classes => classes.WithAttribute<ServiceDescriptorAttribute>())
                              .AsSelfWithInterfaces()
                              .WithSingletonLifetime());

        return services;
    }
}
