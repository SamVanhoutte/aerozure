using OpenTelemetry;
using OpenTelemetry.Logs;
namespace Aerozure.Observability.OpenTelemetry.Filters;
//
// public class LogFilterExporter( AzureMonitorLogExporter innerExporter, Func<LogRecord, bool> filter) : BaseExporter<LogRecord>
// {
//     public override ExportResult Export(in Batch<LogRecord> batch)
//     {
//         var filtered = batch.Where(filter).ToList();
//         if (filtered.Count == 0) return ExportResult.Success;
//
//         // Export filtered logs
//         return innerExporter.Export(new Batch<LogRecord>(filtered, filtered.Count));
//     }
//}