// using Aerozure.Configuration;
// using Aerozure.Observability.Enrichers;
// using Microsoft.ApplicationInsights.Extensibility;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Serilog;
//
// namespace Aerozure.Observability.Serilog;
//
// public static class SerilogConfigurationExtensions
// {
//     public static IServiceCollection ConfigureSerilog(this IServiceCollection services, LoggingConfigurationOptions options)
//     {
//         services.AddSingleton<ILoggingLevelSwitchService, LoggingLevelSwitchService>();
//         services.AddSingleton<TelemetryConfiguration>();
//         
//         services
//             .AddSerilog((services,loggerConfiguration) =>
//                 {
//                     var levelSwitch = services.GetRequiredService<ILoggingLevelSwitchService>().LevelSwitch;
//
//                     loggerConfiguration
//                         .MinimumLevel.ControlledBy(levelSwitch)
//                         .Enrich.With(new ThreadIdEnricher())
//                         .Enrich.WithProperty("Version", "1.0.0")
//                         .WriteTo.Console(
//                             outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
//                         .WriteTo.Debug();
//
//                     if(options.EnableApplicationInsights)
//                     {
//                         var telemetryConfiguration = services.GetRequiredService<TelemetryConfiguration>();
//                         if (!string.IsNullOrWhiteSpace(options.ApplicationInsightsConnectionString))
//                         {
//                             telemetryConfiguration.ConnectionString = options.ApplicationInsightsConnectionString;
//                         }
//                         loggerConfiguration.WriteTo
//                             .ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
//                     }
//                 }
//             );
//         return services;
//     }
//
// }