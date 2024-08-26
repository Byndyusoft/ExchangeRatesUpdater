// ReSharper disable once CheckNamespace

using ExchangeRates.DataAccess;

namespace Microsoft.Extensions.DependencyInjection;

using Byndyusoft.Data.Relational.TypeHandlers;
using Dapper;
using Npgsql;
using Scrutor;

public static class ExchangeRatesDataAccessServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRatesDataAccess(
            this IServiceCollection services,
            string connectionString)
    {
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
        
        services.Scan(scan => scan
                              .FromAssemblies(typeof(ExchangeRatesDataAccessServiceCollectionExtensions).Assembly)
                              .AddClasses(classes => classes.WithAttribute<ServiceDescriptorAttribute>())
                              .AsSelfWithInterfaces()
                              .WithSingletonLifetime());

        services.AddRelationalDb(Repository.SessionKey, NpgsqlFactory.Instance, connectionString);

        return services;
    }
}
