namespace Infrastructure.Tracing;

using System;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class ServiceCollectionTracingExtensions
{
    public static IServiceCollection AddWorkerTracing(
        this IServiceCollection services,
        string serviceName,
        Action<JaegerExporterOptions> jaegerConfig)
    {
        return services.AddApiTracing(serviceName, jaegerConfig);
    }

    public static IServiceCollection AddApiTracing(
        this IServiceCollection services,
        string serviceName,
        Action<JaegerExporterOptions> jaegerConfig)
    {
        return services.AddTracing(
            serviceName,
            jaegerConfig,
            openTelemetry =>
            {
                openTelemetry.AddAspNetCoreInstrumentation(
                    aspNet =>
                    {
                        aspNet.RecordException = true;
                        aspNet.Filter = context =>
                            context.Request.Path.StartsWithSegments("/swagger") == false &&
                            context.Request.Path.StartsWithSegments("/metrics") == false;
                    });
            });
    }

    public static IServiceCollection AddTracing(
        this IServiceCollection services,
        string serviceName,
        Action<JaegerExporterOptions> jaegerConfig,
        Action<TracerProviderBuilder>? builderConfig = null)
    {
        //services.AddSingleton(new ActivitySource(serviceName));

        services.AddSingleton(sp => sp.GetRequiredService<IActivitySourceFactory>().CreateActivitySource());

        services.AddSingleton<IActivitySourceFactory>(_ => new ActivitySourceFactory(serviceName));

        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                    .AddHttpClientInstrumentation(http => { http.RecordException = true; })
                    .AddSqlClientInstrumentation(sql =>
                    {
                        sql.SetDbStatementForStoredProcedure = true;
                        sql.SetDbStatementForText = true;
                        sql.RecordException = true;
                    })
                    .AddDataSessions()
                    .AddRabbitMqClientInstrumentation()
                    .AddRedisInstrumentation()
                    .AddNpgsql()
                    .AddDataSessions()
                    .AddSource(serviceName)
                    .AddJaegerExporter(jaegerConfig);
                builderConfig?.Invoke(tracing);
            })
            .WithMetrics(
                // see https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-metrics-and-dotnet-part-2/
                metrics =>
                {
                    metrics.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName));
                    metrics.AddAspNetCoreInstrumentation();
                    metrics.AddHttpClientInstrumentation();
                    // https://githubissues.com/open-telemetry/opentelemetry-dotnet-contrib/1617
                    metrics.AddPrometheusExporter();
                    metrics.AddRuntimeInstrumentation();
                    metrics.AddProcessInstrumentation();
                    metrics.AddMeter("Npgsql"); // waiting for https://github.com/npgsql/npgsql/pull/5511
                });

        return services;
    }
}