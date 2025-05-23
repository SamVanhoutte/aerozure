using System.Diagnostics;
using OpenTelemetry;

namespace Aerozure.Observability.OpenTelemetry.Filters;


public class AzureMonitorActivityFilterProcessor : BaseProcessor<Activity>
{
    // The OnStart method is called when an activity is started. This is the ideal place to filter activities.
    public override void OnStart(Activity activity)
    {
        Console.WriteLine($"Start - {activity.DisplayName} - {activity.Kind} - {activity.Status} - {activity.Tags.Count()}");
        // prevents all exporters from exporting internal activities
        if (activity.Kind == ActivityKind.Internal)
        {
            SkipActivity(ref activity);
        }

        foreach (var tag in activity.Tags)
        {
            Console.WriteLine($"{tag.Key}: {tag.Value}");
        }

        if (activity.DisplayName == "System.Net.Http.HttpRequestOut")
        {
            SkipActivity(ref activity);
        }
        
        // Skip exporter-related telemetry
        if (activity.DisplayName.Contains("Export") &&
            activity.Kind == ActivityKind.Client &&
            activity.Tags.Any(t => t.Value?.ToString()?.Contains("opentelemetry.proto") == true))
        {
            SkipActivity(ref activity);
        }

        // Filter out dependencies to Azure Monitor diagnostics endpoints
        if (activity.Kind == ActivityKind.Client &&
            activity.DisplayName.Contains("livediagnostics.monitor.azure.com", StringComparison.OrdinalIgnoreCase))
        {
            SkipActivity(ref activity);
        }
        
    }
    
    public override void OnEnd(Activity activity)
    {
        Console.WriteLine($"End - {activity.DisplayName} - {activity.Kind} - {activity.Status} - {activity.Tags.Count()}");
        if (activity.Kind == ActivityKind.Client && activity.Status == ActivityStatusCode.Ok)
        {
            SkipActivity(ref activity);
        }
    }

    private void SkipActivity(ref Activity activity)
    {
        // Bitwise operations
        // This operation clears (sets to 0) the Recorded flag in the ActivityTraceFlags property
        // while leaving other flags unchanged. It ensures that the Recorded flag is not set for the Activity.
        activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
        activity.IsAllDataRequested = false; // Prevent export
    }
}