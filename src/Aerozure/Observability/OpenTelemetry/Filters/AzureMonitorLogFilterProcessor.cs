using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace Aerozure.Observability.OpenTelemetry.Filters;

/// <summary>
/// Optional OTel log processor that drops records below a configurable minimum level before
/// they reach the exporter. Primary filtering should be done via
/// <c>ILoggingBuilder.AddFilter&lt;OpenTelemetryLoggerProvider&gt;()</c> in the host setup,
/// which is more efficient (prevents record creation entirely). Register this processor only
/// when a secondary export-time gate is needed.
/// </summary>
public class AzureMonitorLogFilterProcessor(LogLevel minimumLevel = LogLevel.Warning)
    : BaseProcessor<LogRecord>
{
    // OnStart is not useful for log filtering — log records have already been created by the time
    // any processor sees them. All filtering must happen in OnEnd.

    public override void OnEnd(LogRecord log)
    {
        if (log.LogLevel < minimumLevel)
            return; // Do not call base.OnEnd — this drops the record from the export pipeline.

        base.OnEnd(log);
    }
}
