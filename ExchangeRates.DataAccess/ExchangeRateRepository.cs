using Byndyusoft.Data.Relational;
using ExchangeRates.DataAccess.QueryObjects;
using ExchangeRates.Domain;
using ExchangeRates.Domain.Interfaces;
using JetBrains.Annotations;
using Scrutor;

namespace ExchangeRates.DataAccess;

[ServiceDescriptor, UsedImplicitly]
public class ExchangeRateRepository : Repository, IExchangeRateRepository
{
    public ExchangeRateRepository(IDbSessionAccessor sessionAccessor) : base(sessionAccessor)
    {
    }

    public async Task<ExchangeRate[]> ListByDates(
            DateOnly[] dates, 
            CancellationToken cancellationToken)
    {
        if (dates.Any() == false)
            return Array.Empty<ExchangeRate>();

        var query = ExchangeRateSelect.ByDates(dates);

        var items = await Session.QueryAsync<ExchangeRate>(
                query,
                cancellationToken: cancellationToken);
        return items.ToArray();
    }

    public async Task Update(ExchangeRate[] exchangeRates, CancellationToken cancellationToken)
    {
        if (exchangeRates.Any() == false)
            return;

        foreach (var exchangeRate in exchangeRates)
        {
            var query = ExchangeRateSelect.ByDateAndCurrency(
                    exchangeRate.OnDate,
                    exchangeRate.CurrencyCode);
            var existsExchange = await Session
                    .QuerySingleOrDefaultAsync<ExchangeRate>(
                                    query,
                                    cancellationToken: cancellationToken
                            );

            if (existsExchange == null)
            {
                await Session
                        .ExecuteAsync(ExchangeRateInsert.New(exchangeRate), cancellationToken: cancellationToken);
                continue;
            }

            if (Math.Abs(existsExchange.NominalRate - exchangeRate.NominalRate) >= 0.01M)
            {
                var command = 
                        ExchangeRateUpdate.Update(existsExchange.Id, exchangeRate);
                await Session.ExecuteAsync(
                        command, 
                        cancellationToken: cancellationToken);
            }
        }
    }
}
