using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace Aerozure.Observability.OpenTelemetry.Filters;

public class AzureMonitorLogFilterProcessor : BaseProcessor<LogRecord>
{
    // The OnStart method is called when an activity is started. This is the ideal place to filter activities.
    public override void OnStart(LogRecord log)
    {
        if(log.LogLevel <= LogLevel.Information)
        {
            // What to set here, so that the log is not sent to the Azure Monitor?
        }
    }

    public override void OnEnd(LogRecord log)
    {
        if(log.LogLevel <= LogLevel.Information)
        {
            // What to set here, so that the log is not sent to the Azure Monitor?
        }
        base.OnEnd(log);
    }
}