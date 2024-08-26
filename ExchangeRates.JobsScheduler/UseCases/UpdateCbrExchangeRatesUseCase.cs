using System.Threading;
using System.Threading.Tasks;
using Byndyusoft.Data.Sessions;
using ExchangeRates.Domain.Interfaces;
using ExchangeRates.JobsScheduler.Cbr.Client;
using ExchangeRates.JobsScheduler.Cbr.Interfaces;
using ExchangeRates.JobsScheduler.UseCases.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace ExchangeRates.JobsScheduler.UseCases;

[ServiceDescriptor]
class UpdateCbrExchangeRatesUseCase : IUpdateCbrExchangeRatesUseCase
{
    private readonly ISessionFactory _sessionFactory;
    private readonly IDateTimeService _dateTimeService;
    private readonly ICbrExchangeRatesClient _cbrExchangeRatesClient;
    private readonly ICbrExchangeRateFactory _cbrExchangeRateFactory;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<UpdateCbrExchangeRatesUseCase> _logger;

    public UpdateCbrExchangeRatesUseCase(
            ISessionFactory sessionFactory, 
            IDateTimeService dateTimeService, 
            ICbrExchangeRatesClient cbrExchangeRatesClient, 
            ICbrExchangeRateFactory cbrExchangeRateFactory, 
            IExchangeRateService exchangeRateService, 
            ILogger<UpdateCbrExchangeRatesUseCase> logger)
    {
        _sessionFactory = sessionFactory;
        _dateTimeService = dateTimeService;
        _cbrExchangeRatesClient = cbrExchangeRatesClient;
        _cbrExchangeRateFactory = cbrExchangeRateFactory;
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }


    public async Task Execute(CancellationToken cancellationToken)
    {
        var today = _dateTimeService.GetToday();

        var dates = new[]
                    {
                            today.AddDays(-1),
                            today,
                            today.AddDays(1)
                    };

        var cbrExchangeRates = await _cbrExchangeRatesClient.ListDateRates(
                dates,
                cancellationToken);

        await using var session = _sessionFactory.CreateCommittableSession();

        var exchangeRates = _cbrExchangeRateFactory.Create(cbrExchangeRates);
        foreach (var rate in exchangeRates)
        {
            _logger.LogInformation("Date {Date}, currency {Currency}, rate {Rate}", rate.OnDate, rate.CurrencyCode, rate.Rate);
        }

        await _exchangeRateService.Update(
                exchangeRates,
                cancellationToken);

        await session.CommitAsync(cancellationToken);

    }
}
