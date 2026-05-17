using Aerozure.Commands;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
namespace Aerozure.Aspire;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder RegisterAspireServiceBus(this IHostApplicationBuilder builder, string serviceBusName) 
    {
        builder.AddAzureServiceBusClient(serviceBusName, settings => settings.DisableTracing = false);

        // Ensure topic exists
        return builder;
    }

    public static async Task ConfigureAndStartCommandProcessorAsync<TCommand, TProcessor>(this IHostApplicationBuilder builder, string serviceName, string? commandType = null)
            where TCommand : AeroCommand 
            where TProcessor : CommandProcessor<TCommand>
            
    {
         await builder
             .RegisterAspireServiceBus(serviceName)
             .SetupCommandInfrastructure<TCommand>(serviceName, commandType);
         builder.Services.AddHostedService<TProcessor>();
    }
}