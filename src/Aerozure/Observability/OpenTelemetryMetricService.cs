using System.Diagnostics;
using System.Diagnostics.Metrics;
using Aerozure.Configuration;
using Aerozure.Extensions;
using Aerozure.Tracing;
using Flurl.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aerozure.Observability;

public class OpenTelemetryMetricService(IOptions<LoggingConfigurationOptions> loggingOptions) : IMetricService
{
    private IDictionary<string, Meter> meters = new Dictionary<string, Meter>();
    private IDictionary<string, object> counters = new Dictionary<string, object>();
    private IDictionary<string, object> gauges = new Dictionary<string, object>();

    public void LogCustomCounterMetric<T>(string name, T value, Dictionary<string, object>? context) where T : struct
    {
        LogCustomCounterMetric(name, value, DateTimeOffset.UtcNow, context);
    }

    public void LogCustomCounterMetric<T>(string name, T value, object? context = null) where T : struct
    {
        LogCustomCounterMetric(name, value, DateTimeOffset.UtcNow, context.GetTraceContext());
    }

    public void LogCustomCounterMetric<T>(string name, T value, DateTimeOffset timestamp, object? context = null) where T : struct
    {
        LogCustomCounterMetric(name, value, timestamp, context.GetTraceContext());
    }

    public void LogCustomCounterMetric<T>(string name, T value, DateTimeOffset timestamp,
        Dictionary<string, object>? context)where T : struct
    {
        ArgumentNullException.ThrowIfNull(loggingOptions, nameof(loggingOptions));
        ArgumentNullException.ThrowIfNull(loggingOptions.Value, nameof(loggingOptions.Value));
        ArgumentException.ThrowIfNullOrWhiteSpace(loggingOptions.Value.ServiceName, nameof(loggingOptions.Value.ServiceName));
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        var meter = GetMeter(loggingOptions.Value.ServiceName);
        var counter = GetCounter<T>(name, meter);
        var tags = new TagList(context.ToArray());
        counter.Add(value, tags);
    }
    
    public void LogCustomGaugeMetric<T>(string name, T value, object? context = null) where T : struct
    {
        LogCustomGaugeMetric(name, value, context.GetTraceContext());
    }

    
    public void LogCustomGaugeMetric<T>(string name, T value, 
        Dictionary<string, object>? context)where T : struct
    {
        ArgumentNullException.ThrowIfNull(loggingOptions, nameof(loggingOptions));
        ArgumentNullException.ThrowIfNull(loggingOptions.Value, nameof(loggingOptions.Value));
        ArgumentException.ThrowIfNullOrWhiteSpace(loggingOptions.Value.ServiceName, nameof(loggingOptions.Value.ServiceName));
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        var meter = GetMeter(loggingOptions.Value.ServiceName);
        var gauge = GetGauge<T>(name, meter);
        var tags = new TagList();
        foreach (var kvp in context)
        {
            tags.Add(kvp);
        }
        gauge.Record(value, tags);
    }



    private Meter GetMeter(string name)
    {
        return meters.GetOrAdd(name, new Meter(name));
    }
    
    private Counter<T> GetCounter<T>(string name, Meter meter) where T : struct
    {
        var counter = counters.GetOrAdd(name, meter.CreateCounter<T>(name));
        if (counter is Counter<T> castedCounter)
        {
            return castedCounter;
        }

        throw new InvalidOperationException($"Counter {name} is not of type {typeof(Counter<T>)}");
    }
    
    private Gauge<T> GetGauge<T>(string name, Meter meter) where T : struct
    {
        var gauge = gauges.GetOrAdd(name, meter.CreateGauge<T>(name));
        if (gauge is Gauge<T> castedGauge)
        {
            return castedGauge;
        }

        throw new InvalidOperationException($"Gauge {name} is not of type {typeof(Gauge<T>)}");
    }
}