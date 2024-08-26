using Byndyusoft.Data.Relational;
using ExchangeRates.Domain;

namespace ExchangeRates.DataAccess.QueryObjects;

public static class ExchangeRateInsert
{
    public static QueryObject New(ExchangeRate exchangeRate)
    {
        return new QueryObject($@"
INSERT INTO exchange_rate (
    on_date,
    hash,
    currency_code,
    nomination,
    rate,
    nominal_rate
)
VALUES (
    @{nameof(ExchangeRate.OnDate)},
    @{nameof(ExchangeRate.Hash)},
    @{nameof(ExchangeRate.CurrencyCode)},
    @{nameof(ExchangeRate.Nomination)},
    @{nameof(ExchangeRate.Rate)},
    @{nameof(ExchangeRate.NominalRate)}
)",
                               new
                               {
                                       exchangeRate.OnDate,
                                       exchangeRate.Hash,
                                       CurrencyCode = exchangeRate.CurrencyCode.ToString(),
                                       exchangeRate.Nomination,
                                       exchangeRate.Rate,
                                       exchangeRate.NominalRate
                               });
    }
}
