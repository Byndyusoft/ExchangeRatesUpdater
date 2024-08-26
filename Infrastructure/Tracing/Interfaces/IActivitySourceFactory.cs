namespace Infrastructure.Tracing.Interfaces;

using System.Diagnostics;

public interface IActivitySourceFactory
{
    ActivitySource CreateActivitySource();
}
