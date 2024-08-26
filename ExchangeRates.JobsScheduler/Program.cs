using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace ExchangeRates.JobsScheduler;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(x => x.ClearProviders())
                .UseDefaultServiceProvider((_, options) =>
                {
                    // TODO: отключаем валидацию регистрации сервисов, которые тут не нужны
                    options.ValidateOnBuild = false;
                    options.ValidateScopes = false;
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                                                        .SuppressStatusMessages(true)
                                                        .UseNLog()
                                                        .UseStartup<Startup>());
}
