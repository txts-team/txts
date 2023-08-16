using txts.Types.Interfaces;

namespace txts;

public class Startup : IWebHostStartup
{
    public void Configure(IApplicationBuilder application)
    {
        application.UseForwardedHeaders();
        application.UseHttpLogging();

        application.UseExceptionHandler("/error/500");
        application.UseStatusCodePagesWithReExecute("/error/{0}");

        application.UseRouting();
        application.UseStaticFiles();
        application.UseEndpoints(endpoints => endpoints.MapRazorPages());
    }

    public void ConfigureServices(IServiceCollection services) => services.AddRazorPages();
}