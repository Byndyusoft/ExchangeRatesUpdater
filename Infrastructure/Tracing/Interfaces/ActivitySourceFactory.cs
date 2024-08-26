namespace Infrastructure.Tracing.Interfaces;

using System;
using System.Diagnostics;

public class ActivitySourceFactory : IActivitySourceFactory
{
    private readonly Lazy<ActivitySource> _lazy;

    public ActivitySourceFactory(string name, string? version = null)
    {
        _lazy = new Lazy<ActivitySource>(() => new ActivitySource(name, version));
    }

    public ActivitySource CreateActivitySource() => _lazy.Value;
}
