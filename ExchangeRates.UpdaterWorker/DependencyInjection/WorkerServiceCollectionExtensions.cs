// ReSharper disable once CheckNamespace

using ExchangeRatesUpdaterWorker.HostedServices;

namespace Microsoft.Extensions.DependencyInjection;

using Scrutor;
using System.Text;

internal static class WorkerServiceCollectionExtensions
{
    public static IServiceCollection AddWorkerServices(
            this IServiceCollection services)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        services.AddHostedService<ExchangeRatesUpdaterHostedService>();

        services.Scan(scan => scan
                              .FromAssemblies(typeof(WorkerServiceCollectionExtensions).Assembly)
                              .AddClasses(classes => classes.WithAttribute<ServiceDescriptorAttribute>())
                              .AsSelfWithInterfaces()
                              .WithSingletonLifetime());
        return services;
    }
}
