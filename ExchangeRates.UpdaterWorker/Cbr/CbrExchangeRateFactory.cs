using System;
using System.Collections.Generic;
using ExchangeRates.Domain;
using ExchangeRatesUpdaterWorker.Cbr.Client;
using ExchangeRatesUpdaterWorker.Cbr.Interfaces;
using Infrastructure.Services;
using Scrutor;

namespace ExchangeRatesUpdaterWorker.Cbr;

[ServiceDescriptor]
public class CbrExchangeRateFactory : ICbrExchangeRateFactory
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IHashCalculationService _hashCalculationService;

    public CbrExchangeRateFactory(
            IDateTimeService dateTimeService, 
            IHashCalculationService hashCalculationService)
    {
        _dateTimeService = dateTimeService;
        _hashCalculationService = hashCalculationService;
    }

    public ExchangeRate[] Create(
            CbrExchangeRate[] cbrExchangeRates)
    {
        var utcNow = _dateTimeService.GetUtcNow();

        var exchangeRates = new List<ExchangeRate>();

        foreach (var cbrExchangeRate in cbrExchangeRates)
        {
            if (Enum.TryParse(cbrExchangeRate.CharCode, out CurrencyCode currencyCode) == false)
                continue;

            var date = cbrExchangeRate.Date;
            var rate = cbrExchangeRate.Value;
            var nomination = cbrExchangeRate.Nominal;
            var hash = _hashCalculationService.Hash($"{date}{nomination}{currencyCode}{rate}");

            var exchangeRate = new ExchangeRate
            {
                OnDate = date,
                Rate = rate,
                Hash = hash,
                Nomination = nomination,
                CurrencyCode = currencyCode,
                CreatedAt = utcNow
            };

            exchangeRates.Add(exchangeRate);
        }

        return exchangeRates.ToArray();
    }
}
