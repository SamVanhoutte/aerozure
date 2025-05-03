using Aerozure.Interfaces;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace Aerozure.Commands;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceBusMessagingComponents(this IServiceCollection services)
    {
        services.AddSingleton<IMessageGenerator<ServiceBusMessage>, ServiceBusMessageGenerator>();
        services.AddSingleton<ICommandTransmitter, ServiceBusTransmitter>();
        services.AddSingleton<IEventPublisher, ServiceBusTransmitter>();

        return services;
    }
    
    public static IServiceCollection AddDebugMessagingComponents(this IServiceCollection services)
    {
        services.AddSingleton<IMessageGenerator<string>, StringMessageGenerator>();
        services.AddSingleton<ICommandTransmitter, DebugEventTransmitter>();
        services.AddSingleton<IEventPublisher, DebugEventTransmitter>();

        return services;
    }
}