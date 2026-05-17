using System.Diagnostics;
using OpenTelemetry;

namespace Aerozure.Observability.Extensions;

public class FilteringActivityProcessor : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        // Filter out dependencies to Azure Monitor diagnostics endpoints
        if (activity.Kind == ActivityKind.Client &&
            activity.DisplayName.Contains("livediagnostics.monitor.azure.com", StringComparison.OrdinalIgnoreCase))
        {
            activity.IsAllDataRequested = false; // Prevent export
        }
    }
}