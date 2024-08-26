using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRates.JobsScheduler.UseCases.Interfaces;
using Hangfire;
using Infrastructure.Scheduling;
using Infrastructure.Tracing.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.JobsScheduler.Jobs;

public sealed class UpdateCbrExchangeRatesJob : IRecurringJob
{
    public const string Name = "UpdateCbrExchangeRates";

    private readonly ActivitySource _activitySource;
    private readonly IUpdateCbrExchangeRatesUseCase _useCase;
    private readonly ILogger<UpdateCbrExchangeRatesJob> _logger;

    public UpdateCbrExchangeRatesJob(
        IActivitySourceFactory activitySourceFactory,
        IUpdateCbrExchangeRatesUseCase useCase,
        ILogger<UpdateCbrExchangeRatesJob> logger)
    {
        _useCase = useCase;
        _logger = logger;
        _activitySource = activitySourceFactory.CreateActivitySource();
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task Execute(CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity(nameof(UpdateCbrExchangeRatesJob));

        try
        {
            await _useCase.Execute(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
    }
}