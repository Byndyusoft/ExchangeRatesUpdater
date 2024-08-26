namespace Infrastructure.Scheduling;

using Microsoft.Extensions.DependencyInjection;

public class RecurringJobsBuilder
{
    private readonly IServiceCollection _services;

    public RecurringJobsBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public RecurringJobsBuilder AddJob<TRecurringJob>(
            string cronExpression)
            where TRecurringJob : class, IRecurringJob
    {
        return AddJob<TRecurringJob>(
                typeof(TRecurringJob).Name,
                cronExpression);
    }

    public RecurringJobsBuilder AddJob<TRecurringJob>(
            string jobId,
            string cronExpression)
            where TRecurringJob : class, IRecurringJob
    {
        _services.AddSingleton<TRecurringJob>();
        _services.PostConfigure<RecurringJobsOptions>(
                options => options.Jobs.Add(new RecurringJobRegistration<TRecurringJob>
                                            {
                                                    CronExpression = cronExpression,
                                                    JobId = jobId
                                            }));
        return this;
    }

   
}
