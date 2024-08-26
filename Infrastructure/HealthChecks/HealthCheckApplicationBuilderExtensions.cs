// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

using Diagnostics.HealthChecks;
using Infrastructure.HealthChecks;

public static class HealthCheckApplicationBuilderExtensions
{
    public static IApplicationBuilder UseReadinessProbe(
        this IApplicationBuilder builder)
    {
            return builder.UseHealthChecks(
                    "/_readiness",
                    new HealthCheckOptions
                    {
                            AllowCachingResponses = false,
                            Predicate = check => check.Tags.Contains(Probes.Readiness)
                    });
        }

    public static IApplicationBuilder UseLivenessProbe(
        this IApplicationBuilder builder)
    {
            return builder.UseHealthChecks(
                    "/_healthz",
                    new HealthCheckOptions
                    {
                            AllowCachingResponses = false,
                            Predicate = check => check.Tags.Contains(Probes.Liveness)
                    });
        }

    public static IApplicationBuilder UseStartupProbe(
        this IApplicationBuilder builder)
    {
            return builder.UseHealthChecks(
                    "/_startup",
                    new HealthCheckOptions
                    {
                            AllowCachingResponses = false,
                            Predicate = check => check.Tags.Contains(Probes.Startup)
                    });
        }
}