namespace txts.Types.Interfaces;

public interface IWebHostStartup
{
    public void Configure(IApplicationBuilder application);
    public void ConfigureServices(IServiceCollection services);
}