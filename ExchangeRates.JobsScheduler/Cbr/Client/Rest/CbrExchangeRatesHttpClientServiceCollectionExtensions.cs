

using ExchangeRates.JobsScheduler.Cbr.Client.Rest;
// ReSharper disable once CheckNamespace
using ExchangeRates.JobsScheduler.Cbr.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class CbrExchangeRatesHttpClientServiceCollectionExtensions
{
    public static IServiceCollection AddCbrExchangeRatesHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<ICbrExchangeRatesClient, CbrExchangeRatesHttpClient>();
        return services;
    }
}
