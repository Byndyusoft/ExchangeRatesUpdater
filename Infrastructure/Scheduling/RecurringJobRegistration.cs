namespace Infrastructure.Scheduling;

using System.Threading;
using Hangfire;
using Hangfire.Common;

internal record RecurringJobRegistration<T> : IRecurringJobRegistration
        where T : IRecurringJob
{
    public string JobId { get; init; } = default!;

    public string CronExpression { get; init; } = default!;

    public void Register(IRecurringJobManager manager, CancellationToken cancellationToken)
    {
        manager.AddOrUpdate(
                JobId,
                Job.FromExpression<T>(recurringJob => recurringJob.Execute(cancellationToken)),
                CronExpression);
    }
}
