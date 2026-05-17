using Aerozure.Interfaces;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aerozure.Commands;

public static class ServiceCollectionExtensions
{
    public static async Task SetupCommandInfrastructure<T>(this IHostApplicationBuilder builder, string serviceBusName, string? commandType = null)
        where T : AeroCommand
    {
        var serviceBusNamespace = builder.Configuration.GetConnectionString(serviceBusName);
        
        var fullyQualifiedNamespace = serviceBusNamespace.Replace("https://", string.Empty).Replace(":443/", string.Empty);
        var credential = new DefaultAzureCredential();
        var adminClient = new ServiceBusAdministrationClient(fullyQualifiedNamespace, credential);
        
        await EnsureTopicExists(adminClient, ServiceBusConstants.CommandsTopicName);
        await EnsureSubscriptionExists(adminClient, ServiceBusConstants.CommandsTopicName, AeroCommand.GetSubscriptionName<T>(), commandType);
    }



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

    private static async Task EnsureTopicExists(ServiceBusAdministrationClient adminClient, string topicName)
    {
        if (!await adminClient.TopicExistsAsync(topicName))
        {
            await adminClient.CreateTopicAsync(new CreateTopicOptions(topicName)
            {
                // Optional configuration
                DefaultMessageTimeToLive = TimeSpan.FromDays(7),
                RequiresDuplicateDetection = true,
                DuplicateDetectionHistoryTimeWindow = TimeSpan.FromSeconds(30),
                Status = EntityStatus.Active
            });
        }
    }
    
    private static async Task EnsureSubscriptionExists(ServiceBusAdministrationClient adminClient, string topicName, string subscriptionType, string? commandType)
    {
        var topic = await adminClient.GetTopicAsync(topicName);
        if (topic != null)
        {
            if (!await adminClient.SubscriptionExistsAsync(topicName, subscriptionType))
            {
                commandType ??= subscriptionType;
                var subscription = await adminClient.CreateSubscriptionAsync(new CreateSubscriptionOptions(topicName, subscriptionType)
                {
                    // Optional configuration
                    DefaultMessageTimeToLive = TimeSpan.FromDays(7),
                    Status = EntityStatus.Active,
                });
                // Remove the default rule
                await adminClient.DeleteRuleAsync(topicName, subscriptionType, "$Default");

                // Add a custom SQL filter rule
                await adminClient.CreateRuleAsync(topicName, subscriptionType, new CreateRuleOptions
                {
                    Name = "CommandTypeFilter",
                    Filter = new SqlRuleFilter($"{ServiceBusConstants.CommandTypePropertyName} = '{commandType}'")
                });
            }
        }
    }
}