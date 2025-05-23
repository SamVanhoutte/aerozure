// using Aerozure.Tracing;
// using Microsoft.ApplicationInsights;
//
// namespace Aerozure.Observability;
//
// public class TelemetryClientMonitorService(TelemetryClient telemetryClient) : IMonitorService
// {
//     public void LogCustomEvent(string name, Dictionary<string, object>? context )
//     {
//         ArgumentNullException.ThrowIfNull(telemetryClient, nameof(telemetryClient));
//         ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
//         telemetryClient.TrackEvent(name, context?.ToDictionary(k => k.Key, v => v.Value.ToString()));
//     }
//
//     public void LogCustomEvent(string name, object? context = null)
//     {
//         LogCustomEvent(name, context.GetTraceContext());
//     }
//
//     public void LogCustomMetric(string name, double value, Dictionary<string, object>? context)
//     {
//         LogCustomMetric(name, value, DateTimeOffset.UtcNow, context);
//     }
//
//     public void LogCustomMetric(string name, double value, object? context = null)
//     {
//         LogCustomMetric(name, value, context.GetTraceContext());
//     }
//
//     public void LogCustomMetric(string name, double value, DateTimeOffset timestamp, Dictionary<string, object>? context )
//     {
//         ArgumentNullException.ThrowIfNull(telemetryClient, nameof(telemetryClient));
//         ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
//         telemetryClient.TrackMetric(name, value, context?.ToDictionary(k => k.Key, v => v.Value.ToString()));
//     }
//
//     public void LogCustomMetric(string name, double value, DateTimeOffset timestamp, object? context = null)
//     {
//         LogCustomMetric(name, value, timestamp, context.GetTraceContext());
//     }
// }