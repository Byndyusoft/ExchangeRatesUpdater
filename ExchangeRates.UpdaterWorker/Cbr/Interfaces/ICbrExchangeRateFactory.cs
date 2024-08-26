using ExchangeRates.Domain;
using ExchangeRatesUpdaterWorker.Cbr.Client;

namespace ExchangeRatesUpdaterWorker.Cbr.Interfaces;

public interface ICbrExchangeRateFactory
{
    ExchangeRate[] Create(
            CbrExchangeRate[] cbrExchangeRates);
}
