namespace Infrastructure.Scheduling;

using Hangfire;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

public class RegisterRecurringJobsHostedService : BackgroundService
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly RecurringJobsOptions _jobOptions;
    private readonly IHostApplicationLifetime _applicationLifetime;

    public RegisterRecurringJobsHostedService(
            IRecurringJobManager recurringJobManager,
            IOptions<RecurringJobsOptions> jobOptions,
            IHostApplicationLifetime applicationLifetime)
    {
        _recurringJobManager = recurringJobManager;
        _applicationLifetime = applicationLifetime;
        _jobOptions = jobOptions.Value;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var recurringJob in _jobOptions.Jobs)
        {
            recurringJob.Register(
                    _recurringJobManager, 
                    _applicationLifetime.ApplicationStopping);
        }

        return Task.CompletedTask;
    }
}
