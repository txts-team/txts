using Serilog;
using txts.Logging;

namespace txts;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration().WriteTo
            .Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [Core] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        await StartupTasks.MigrateDatabase();

        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .UseSerilog((_, serviceProvider, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Services(serviceProvider);
            loggerConfiguration.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [ASP.NET] [{TraceId}] <{Stack}> {Message:lj}{NewLine}{Exception}");
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseWebRoot("StaticFiles");
            webBuilder.UseStartup<Startup>();
        });
}