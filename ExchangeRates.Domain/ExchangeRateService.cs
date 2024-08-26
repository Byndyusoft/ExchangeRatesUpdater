using ExchangeRates.Domain.Interfaces;
using Scrutor;

namespace ExchangeRates.Domain;

/// <summary>
///     Сервис курса валют
/// </summary>
[ServiceDescriptor]
public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public ExchangeRateService(IExchangeRateRepository exchangeRateRepository)
    {
        _exchangeRateRepository = exchangeRateRepository;
    }

    /// <summary>
    ///     Получение курса валют на дату
    /// </summary>
    public Task<ExchangeRate[]> ListByDate(
            DateOnly date,
            CancellationToken cancellationToken)
    {
        return ListByDates(
                new[] { date },
                cancellationToken);
    }
    /// <summary>
    ///     Получение курса валют на дату
    /// </summary>
    public async Task<ExchangeRate[]> ListByDates(
            DateOnly[] dates,
            CancellationToken cancellationToken)
    {
        return await _exchangeRateRepository.ListByDates(
                dates,
                cancellationToken);
    }

    /// <summary>
    ///     Обновление курса валют
    /// </summary>
    public async Task Update(
            ExchangeRate[] exchangeRates,
            CancellationToken cancellationToken)
    {
        if (exchangeRates.Any() == false)
            return;
        
        await _exchangeRateRepository.Update(
                exchangeRates,
                cancellationToken);
    }
}
