using Byndyusoft.Data.Relational;
using ExchangeRates.Domain;

namespace ExchangeRates.DataAccess.QueryObjects;

public static class ExchangeRateUpdate
{
    public static QueryObject Update(int existingId, ExchangeRate exchangeRate)
    {
        return new QueryObject($@"
UPDATE exchange_rate
SET on_date = @{nameof(ExchangeRate.OnDate)},
    hash = @{nameof(ExchangeRate.Hash)},
    currency_code = @{nameof(ExchangeRate.CurrencyCode)},
    nomination = @{nameof(ExchangeRate.Nomination)},
    rate = @{nameof(ExchangeRate.Rate)},
    nominal_rate = @{nameof(ExchangeRate.NominalRate)}
WHERE
    id = @{nameof(existingId)}",
                               new
                               {
                                       existingId,
                                       exchangeRate.OnDate,
                                       exchangeRate.Hash,
                                       CurrencyCode = exchangeRate.CurrencyCode.ToString(),
                                       exchangeRate.Nomination,
                                       exchangeRate.Rate,
                                       exchangeRate.NominalRate
                               });
    }
}
