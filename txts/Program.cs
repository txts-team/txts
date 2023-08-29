using Serilog;

namespace txts;

public static class Program
{
    public static async Task Main(string[] args)
    {
        #region Startup tasks

        await StartupTasks.MigrateDatabase();

        #endregion

        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .UseSerilog((_, serviceProvider, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Services(serviceProvider);
            loggerConfiguration.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [ASP.NET] [{Stack}] <{TraceId}> {Message:lj}{NewLine}{Exception}");
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseWebRoot("StaticFiles");
            webBuilder.UseStartup<Startup>();
        });
}