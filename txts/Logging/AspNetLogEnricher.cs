using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace txts.Logging;

public class AspNetLogEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor contextAccessor;

    public AspNetLogEnricher(IHttpContextAccessor contextAccessor)
    {
        this.contextAccessor = contextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        #region Trace identifier stuff

        string? traceIdentifier = this.contextAccessor.HttpContext?.TraceIdentifier;

        #endregion
        
        #region Stacktrace stuff

        StackTrace stackTrace = new();
        StackFrame? stackFrame = stackTrace.GetFrame(10);
        string? stackName = stackFrame?.GetMethod()?.DeclaringType?.Name;
        // string? stackSection = stackFrame?.GetMethod()?.Name;
        // string stack = $"{stackName}:{stackSection}".PadRight(40)[..40];
        string stack = $"{stackName}".PadRight(25)[..25];

        #endregion

        LogEventProperty traceIdProperty = propertyFactory.CreateProperty("TraceId", traceIdentifier ?? "None");
        logEvent.AddPropertyIfAbsent(traceIdProperty);
        
        LogEventProperty stackProperty = propertyFactory.CreateProperty("Stack", stack);
        logEvent.AddPropertyIfAbsent(stackProperty);
    }
}