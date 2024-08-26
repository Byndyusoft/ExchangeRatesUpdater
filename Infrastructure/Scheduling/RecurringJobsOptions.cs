namespace Infrastructure.Scheduling;

using System.Collections.Generic;

public class RecurringJobsOptions
{
    internal List<IRecurringJobRegistration> Jobs { get; }= new();
}
