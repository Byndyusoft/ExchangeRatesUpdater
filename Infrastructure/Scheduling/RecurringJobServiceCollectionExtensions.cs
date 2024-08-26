// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

using Hangfire;
using Hangfire.MemoryStorage;
using Infrastructure.Scheduling;

public static class RecurringJobServiceCollectionExtensions
{
    public static RecurringJobsBuilder AddRecurringJobs(
            this IServiceCollection services)
    {
        services.AddHangfireServer();
        services.AddHangfire(
                hangfire =>
                {
                    hangfire.UseMemoryStorage();
                    hangfire.UseNLogLogProvider();
                });

        services.AddHostedService<RegisterRecurringJobsHostedService>();

        return new RecurringJobsBuilder(services);
    }
}
