using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Api;

public static class Program
{

    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseDefaultServiceProvider((_, options) =>
            {
                // TODO: отключаем валидацию регистрации сервисов, которые тут не нужны
                options.ValidateOnBuild = false;
                options.ValidateScopes = false;
            })
            .ConfigureLogging(x => x.ClearProviders())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .SuppressStatusMessages(true)
                    .UseStartup<Startup>();
            });
}