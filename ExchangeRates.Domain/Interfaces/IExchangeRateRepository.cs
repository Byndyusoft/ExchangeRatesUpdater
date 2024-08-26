namespace ExchangeRates.Domain.Interfaces;

public interface IExchangeRateRepository
{
    Task<ExchangeRate[]> ListByDates(
            DateOnly[] dates, 
            CancellationToken cancellationToken);

    Task Update(
            ExchangeRate[] exchangeRates, 
            CancellationToken cancellationToken);
}
