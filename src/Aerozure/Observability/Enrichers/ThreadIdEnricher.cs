// using Serilog.Core;
// using Serilog.Events;
//
// namespace Aerozure.Observability.Enrichers;
//
// public class ThreadIdEnricher : ILogEventEnricher
// {
//     public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
//     {
//         logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
//             "ThreadId", Thread.CurrentThread.ManagedThreadId));
//     }
// }