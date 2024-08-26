namespace ExchangeRates.Domain.Interfaces;

public interface IExchangeRateService
{
    /// <summary>
    ///     Получение курса валют на дату
    /// </summary>
    Task<ExchangeRate[]> ListByDate(
            DateOnly date,
            CancellationToken cancellationToken);

    /// <summary>
    ///     Получение курса валют на дату
    /// </summary>
    Task<ExchangeRate[]> ListByDates(
            DateOnly[] dates,
            CancellationToken cancellationToken);

    /// <summary>
    ///     Обновление курса валют
    /// </summary>
    Task Update(
            ExchangeRate[] exchangeRates,
            CancellationToken cancellationToken);
}
