using System;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRatesUpdaterWorker.UseCases.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRatesUpdaterWorker.HostedServices;

public class ExchangeRatesUpdaterHostedService : BackgroundService
{
    private readonly ILogger<ExchangeRatesUpdaterHostedService> _logger;
    private readonly IUpdateCbrExchangeRatesUseCase _updateCbrExchangeRatesUseCase;

    public ExchangeRatesUpdaterHostedService(
            ILogger<ExchangeRatesUpdaterHostedService> logger,
            IUpdateCbrExchangeRatesUseCase updateCbrExchangeRatesUseCase)
    {
        _logger = logger;
        _updateCbrExchangeRatesUseCase = updateCbrExchangeRatesUseCase;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _updateCbrExchangeRatesUseCase.Execute(stoppingToken);

        _logger.LogInformation("RunExchangeRatesUpdater is running");

        while (stoppingToken.IsCancellationRequested == false)
        {
            try
            {
                _logger.LogInformation("Start UpdateExchangeRates");
                await _updateCbrExchangeRatesUseCase.Execute(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Updating exchange rates error");
            }

            _logger.LogInformation("Sleep for 60 minutes...");
            await Task
                    .Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
