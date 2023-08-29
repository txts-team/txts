using System.Diagnostics;
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
        #region Stacktrace stuff

        StackTrace stackTrace = new();
        StackFrame? stackFrame = stackTrace.GetFrame(10);
        string? stackName = stackFrame?.GetMethod()?.DeclaringType?.Name;
        string? stackSection = stackFrame?.GetMethod()?.Name;
        string stack = $"{stackName}:{stackSection}".PadRight(40)[..40];

        #endregion
        
        string? traceIdentifier = this.contextAccessor.HttpContext?.TraceIdentifier;

        LogEventProperty userNameProperty = propertyFactory.CreateProperty("TraceId", traceIdentifier ?? "None");
        logEvent.AddPropertyIfAbsent(userNameProperty);

        LogEventProperty stackProperty = propertyFactory.CreateProperty("Stack", stack);
        logEvent.AddPropertyIfAbsent(stackProperty);
    }
}