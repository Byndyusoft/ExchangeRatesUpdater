namespace Infrastructure.Scheduling;

using System.Threading;
using System.Threading.Tasks;

public interface IRecurringJob
{
    Task Execute(CancellationToken cancellationToken);
}
