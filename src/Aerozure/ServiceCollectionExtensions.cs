using Aerozure.Aspire;
using Aerozure.Azureml;
using Aerozure.Commands;
using Aerozure.Communication;
using Aerozure.Configuration;
using Aerozure.Encryption;
using Aerozure.Interfaces;
using Aerozure.Observability;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace Aerozure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAerozure(this IServiceCollection services,
        Action<AeroStartupOptions>? configureRuntime = null, string? telemetryActivitySource = null)
    {
        services.AddTransient<AzuremlClient>();
        services.AddSingleton<IEncryptionService, AesEncryptionService>();
        services.AddSingleton<IActivityObserver, ActivityObserver>();
        ConfigureOptions(services, configureRuntime);
        if (!string.IsNullOrEmpty(telemetryActivitySource))
        {
            ObservabilityOptions.TraceActivitySource = telemetryActivitySource;
        }

        return services;
    }

    private static AeroStartupOptions ConfigureOptions(IServiceCollection services,
        Action<AeroStartupOptions>? configureRuntime)
    {
        var options = AeroStartupOptions.Default;
        if (configureRuntime != null)
        {
            configureRuntime(options);
        }

        if (options.EnableCommunication)
        {
            services.AddCommunicationServices();

            // services.Configure<MapOptions>(o =>
            // {
            //     o.GoogleMapKey = options.GoogleMapsConfiguration!.GoogleMapKey;
            // });
        }

        if (options.EnableMessaging && options.MessagingOptions != null)
        {
            switch (options.MessagingOptions.MessagingType)
            {
                case MessagingType.Debug:
                    services.AddDebugMessagingComponents();
                    break;
                case MessagingType.RabbitMq:
                    break;
                case MessagingType.AzureServiceBus:
                    services.AddServiceBusMessagingComponents();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (options.EnableAppInsights)
        {
            // // Register TelemetryConfiguration manually
            // services.AddSingleton<TelemetryConfiguration>(sp =>
            // {
            //     var config = TelemetryConfiguration.CreateDefault();
            //     if (!string.IsNullOrEmpty(options?.LoggingOptions?.ApplicationInsightsConnectionString))
            //     {
            //         config.ConnectionString = options.LoggingOptions.ApplicationInsightsConnectionString;
            //     }
            //
            //     return config;
            // });
            //
            // // Register TelemetryClient
            // services.AddSingleton<TelemetryClient>(sp =>
            //     {
            //         var config = sp.GetRequiredService<TelemetryConfiguration>();
            //         var client = new TelemetryClient(config);
            //         return client;
            //     });
            // services.AddSingleton<I<MonitorService, AzureMonitorService>();
         }

        if (options.EnableAspire)
        {
            services.Configure<AspireHostingOptions>(o =>
                o.ServiceUrlConfigurationKey = options.AspireOptions?.ServiceUrlConfigurationKey);
            services.AddSingleton<IServiceUriResolver, AspireServiceUriResolver>();
        }

        return options;
    }
}