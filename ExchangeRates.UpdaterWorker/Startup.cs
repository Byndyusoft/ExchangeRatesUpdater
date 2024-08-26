using Infrastructure.Tracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRatesUpdaterWorker;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddControllers();

        services
            .AddDataSessions()
            .AddExchangeRatesDomain()
            .AddExchangeRatesDataAccess(Configuration.GetRequiredConnectionString("ExchangeRates")!)
            .AddWorkerServices();

        services.AddHealthChecks();

        services.AddWorkerTracing(
                Configuration.GetServiceName(),
                Configuration.GetSection("Jaeger").Bind);

        services.AddCbrExchangeRatesHttpClient();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapPrometheusScrapingEndpoint();
        });

        app.UseReadinessProbe()
           .UseLivenessProbe();
    }
}
