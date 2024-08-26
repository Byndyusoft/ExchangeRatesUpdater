using ExchangeRates.Domain;
using ExchangeRates.JobsScheduler.Cbr.Client;

namespace ExchangeRates.JobsScheduler.Cbr.Interfaces;

public interface ICbrExchangeRateFactory
{
    ExchangeRate[] Create(
            CbrExchangeRate[] cbrExchangeRates);
}
