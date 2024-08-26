using System;
using System.IO;
using System.Linq;
using Dapper;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ExchangeRates.Migrator;

public class Program
{
    public static ILoggerFactory LoggerFactory = default!;
    public static IConfigurationRoot Configuration = default!;
    public static ServiceProvider ServiceProvider = default!;

    public static void Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        if (string.IsNullOrWhiteSpace(environment))
            throw new ArgumentException("Environment not found in DOTNET_ENVIRONMENT");

        var builder = new ConfigurationBuilder()
                      .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                      .AddJsonFile("appsettings.json", optional: true)
                      .AddJsonFile($"appsettings.{environment}.json", optional: true)
                      .AddEnvironmentVariables();
        Configuration = builder.Build();

        var connectionString = Configuration.GetConnectionString("ExchangeRates");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException(@"ConnectionString ""ExchangeRates"" not found");

        var services = new ServiceCollection()
                .AddLogging(logging =>
                {
                    logging.AddConfiguration(Configuration.GetSection("Logging"))
                           .AddConsole();
                });

        services.AddFluentMigratorCore()
                .ConfigureRunner(runner => runner
                                           .AddPostgres11_0()
                                           .WithGlobalConnectionString(connectionString)
                                           .WithGlobalCommandTimeout(TimeSpan.FromSeconds(600))
                                           .ScanIn(typeof(Program).Assembly).For.Migrations())
                .AddLogging(log => log.AddFluentMigratorConsole())
                .Configure<RunnerOptions>(opt =>
                {
                    opt.AllowBreakingChange = true;
                    opt.Tags = new[] { environment };
                });
        ServiceProvider = services.BuildServiceProvider();

        LoggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();

        if (environment == "Development")
        {
            CreateDatabaseIfNotExists(connectionString);
        }

        Run();
    }

    private static void Run()
    {
        var logger = LoggerFactory.CreateLogger<Program>();
        try
        {
            logger.LogInformation("Migration started");
            var migrator = ServiceProvider.GetRequiredService<IMigrationRunner>();
            migrator.MigrateUp();

            logger.LogInformation("Migration completed");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Migration error");
            throw;
        }
    }

    private static void CreateDatabaseIfNotExists(string connectionString)
    {
        var databaseName = new NpgsqlConnectionStringBuilder(connectionString).Database;

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString) { Database = "postgres" };

        using var connection = new NpgsqlConnection(connectionStringBuilder.ToString());

        if (connection.Query($"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'").Any() == false)
        {
            connection.Execute($"CREATE DATABASE \"{databaseName}\"");
        }
    }
}
