using System.Diagnostics;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using txts.Database;
using txts.Types.Interfaces;

namespace txts;

public class Startup : IWebHostStartup
{
    public void Configure(IApplicationBuilder application)
    {
        application.UseForwardedHeaders();
        application.UseSerilogRequestLogging();

        application.UseRateLimiter();

        application.UseExceptionHandler("/error/500");
        application.UseStatusCodePagesWithReExecute("/error/{0}");

        application.UseRouting();

        application.UseStaticFiles();
        application.UseEndpoints(endpoints => endpoints.MapRazorPages());
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(httpContext.Request.Headers.Host.ToString(), _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        AutoReplenishment = true,
                        Window = TimeSpan.FromSeconds(10),
                    });
            });
            options.OnRejected = async (context, _) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                context.HttpContext.Response.Headers.Add("Retry-After", "10");
                await Task.CompletedTask;
            };
        });
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlite("Data Source=txts.db");
        });
    }
}

public static class StartupTasks
{
    public static async Task MigrateDatabase()
    {
        await using DatabaseContext database = new(new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite("Data Source=txts.db")
            .Options);

        Log.Information("Beginning migration of database {Database}", database.Database.GetDbConnection().Database);

        Stopwatch migrationStopwatch = Stopwatch.StartNew();

        try
        {
            await database.Database.MigrateAsync();
            await database.Database.EnsureCreatedAsync();
        }
        catch (Exception e)
        {
            migrationStopwatch.Stop();

            Log.Error(e, "An exception occurred while creating or migrating database {Database}",
                database.Database.GetDbConnection().Database);
            return;
        }

        migrationStopwatch.Stop();

        Log.Information("Successfully migrated database {Database} in {Time}ms", database.Database.GetDbConnection().Database,
            migrationStopwatch.ElapsedMilliseconds);

        await database.DisposeAsync(); // dispose of the migration context
    }
}