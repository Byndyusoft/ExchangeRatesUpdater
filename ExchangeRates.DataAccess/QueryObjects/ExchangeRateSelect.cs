using Byndyusoft.Data.Relational;
using ExchangeRates.Domain;

namespace ExchangeRates.DataAccess.QueryObjects;

public static class ExchangeRateSelect
{
    private const string ExchangeRateFields = $@"
    er.id AS {nameof(ExchangeRate.Id)},
    er.on_date AS {nameof(ExchangeRate.OnDate)},
    er.created_at AS {nameof(ExchangeRate.CreatedAt)},
    er.hash AS {nameof(ExchangeRate.Hash)},
    er.currency_code AS {nameof(ExchangeRate.CurrencyCode)},
    er.nomination AS {nameof(ExchangeRate.Nomination)},
    er.rate AS {nameof(ExchangeRate.Rate)}";

    public static QueryObject ByDateAndCurrency(DateOnly date, CurrencyCode currencyCode)
    {
        return new QueryObject($@"
SELECT
    {ExchangeRateFields}
FROM
    exchange_rate er
WHERE
    er.on_date = @{nameof(date)}
    AND er.currency_code = @{nameof(currencyCode)}",
                               new
                               {
                                   date,
                                   currencyCode = currencyCode.ToString()
                               });
    }

    public static QueryObject ByDates(DateOnly[] dates)
    {
        return new QueryObject($@"
SELECT
    {ExchangeRateFields}
FROM
    exchange_rate er
WHERE
    er.on_date = ANY(@{nameof(dates)})",
                               new
                               {
                                   dates
                               });
    }
}
