using OpenTelemetry;
using OpenTelemetry.Logs;

namespace Aerozure.Observability.Extensions;

public class FilteringLoggingProcessor : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord activity)
    {
    }
}