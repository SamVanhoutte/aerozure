using Aerozure.Interfaces;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace Aerozure.Commands;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingComponents(this IServiceCollection services)
    {
        services.AddSingleton<IMessageGenerator<ServiceBusMessage>, ServiceBusMessageGenerator>();
        services.AddSingleton<ICommandTransmitter, ServiceBusTransmitter>();
        services.AddSingleton<IEventPublisher, ServiceBusTransmitter>();

        return services;
    }
}