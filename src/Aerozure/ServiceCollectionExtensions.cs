using Aerozure.Azureml;
using Aerozure.Commands;
using Aerozure.Communication;
using Aerozure.Configuration;
using Aerozure.Encryption;
using Microsoft.Extensions.DependencyInjection;

namespace Aerozure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAerozure(this IServiceCollection services,
        Action<AeroStartupOptions>? configureRuntime = null)
    {
        services.AddTransient<AzuremlClient>();
        services.AddSingleton<IEncryptionService, AesEncryptionHelper>();
        ConfigureOptions(services, configureRuntime);
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
        if (options.EnableMessaging)
        {
            services.AddMessagingComponents();
        }
        return options;
    }
}