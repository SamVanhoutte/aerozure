using Aerozure.Commands;
using Aerozure.Communication;
using Aerozure.Configuration;
using CyclingStats.Logic.Prediction;
using Microsoft.Extensions.DependencyInjection;

namespace Aerozure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAerozure(this IServiceCollection services,
        Action<AeroStartupOptions>? configureRuntime = null)
    {
        if (configureRuntime != null)
        {
            ConfigureOptions(services, configureRuntime);
        }
        return services;
    }

    private static AeroStartupOptions ConfigureOptions(IServiceCollection services,
        Action<AeroStartupOptions> configureRuntime)
    {
        var options = AeroStartupOptions.Default;
        configureRuntime(options);
        // if (options.InjectHttpClient)
        // {
        //     services.AddHttpClient();
        // }
        services.AddTransient<AzuremlClient>();

        if (options.EnableCommunication)
        {
            services.AddCommunicationServices();

            // services.Configure<MapOptions>(o =>
            // {
            //     o.GoogleMapKey = options.GoogleMapsConfiguration!.GoogleMapKey;
            // });
        }
        if (options.EnableMessaging)
        {
            services.AddMessagingComponents();
        }
        return options;
    }
}