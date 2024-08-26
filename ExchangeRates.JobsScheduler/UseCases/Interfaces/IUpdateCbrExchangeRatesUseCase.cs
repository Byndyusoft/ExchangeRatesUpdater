using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRates.JobsScheduler.UseCases.Interfaces;

/// <summary>
///     Сценарий использования: Обновить курсы валют и сайта ЦБРФ 
/// </summary>
public interface IUpdateCbrExchangeRatesUseCase
{
    Task Execute(CancellationToken cancellationToken);
}
