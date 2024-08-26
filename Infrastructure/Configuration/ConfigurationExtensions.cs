// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration;

using System;
using System.Diagnostics;

public static class ConfigurationExtensions
{
    public static string GetServiceName(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("SERVICE_NAME") ??
               configuration.GetValue<string>("SERVICENAME") ??
               Process.GetCurrentProcess().ProcessName;
    }

    public static string GetRequiredConnectionString(this IConfiguration configuration, string name)
    {
        var connectionString = configuration.GetConnectionString(name);
        if (connectionString is null)
        {
            throw new InvalidOperationException($"Connection string '{name}' isn't registered");
        }

        return connectionString;
    }
}
