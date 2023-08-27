using Serilog.Core;
using Serilog.Events;

namespace txts.Logging;

public class HttpContextLogEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor contextAccessor;

    public HttpContextLogEnricher(IHttpContextAccessor contextAccessor)
    {
        this.contextAccessor = contextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        string? traceIdentifier = this.contextAccessor.HttpContext?.TraceIdentifier;

        LogEventProperty userNameProperty = propertyFactory.CreateProperty("TraceId", traceIdentifier ?? "None");
        logEvent.AddPropertyIfAbsent(userNameProperty);
    }
}