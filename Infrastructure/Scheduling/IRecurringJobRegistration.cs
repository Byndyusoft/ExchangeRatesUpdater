namespace Infrastructure.Scheduling;

using System.Threading;
using Hangfire;

internal interface IRecurringJobRegistration
{
    void Register(IRecurringJobManager manager, CancellationToken cancellationToken);
}
