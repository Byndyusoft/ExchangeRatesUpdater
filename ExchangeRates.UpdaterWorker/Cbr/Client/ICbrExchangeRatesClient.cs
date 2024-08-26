using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRatesUpdaterWorker.Cbr.Client;

public interface ICbrExchangeRatesClient
{
    Task<CbrExchangeRate[]> ListDateRates(
            DateOnly date,
            CancellationToken cancellationToken);

    async Task<CbrExchangeRate[]> ListDateRates(
            DateOnly[] dates,
            CancellationToken cancellationToken)
    {
        var rates = new List<CbrExchangeRate>();
        foreach (var date in dates)
        {
            var dateRates = await ListDateRates(date, cancellationToken);
            rates.AddRange(dateRates);
        }

        return rates.ToArray();
    }
}
