namespace Aerozure.Observability;

public interface IMetricService
{
    void LogCustomCounterMetric<T>(string name, T value, Dictionary<string, object>? context)where T : struct;
    void LogCustomCounterMetric<T>(string name, T value, object? context = null)where T : struct;
    void LogCustomCounterMetric<T>(string name, T value, DateTimeOffset timestamp, Dictionary<string, object>? context)where T : struct;
    void LogCustomCounterMetric<T>(string name, T value, DateTimeOffset timestamp, object? context = null)where T : struct;

    void LogCustomGaugeMetric<T>(string name, T value, Dictionary<string, object>? context) where T : struct;
    void LogCustomGaugeMetric<T>(string name, T value, object? context = null) where T : struct;
}