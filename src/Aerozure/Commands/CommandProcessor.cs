using Aerozure.Configuration;
using Aerozure.Interfaces;
using Aerozure.Observability;
using Aerozure.Tracing;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Metrics = Aerozure.Observability.Metrics;

namespace Aerozure.Commands;

public abstract class CommandProcessor<T>(
    ServiceBusClient client,
    ILogger<CommandProcessor<T>> logger,
    IMetricService metricService,
    IOptions<MessagingOptions> messagingOptions) : BackgroundService where T : AeroCommand
{
    // the processor that reads and processes messages from the queue
    protected ServiceBusProcessor processor;
    protected ServiceBusSender resubmitter;
    private MessagingOptions messagingSettings => messagingOptions.Value;
    private string SubscriptionName => AeroCommand.GetSubscriptionName<T>();

    protected abstract Task<MessageOption> ProcessCommandAsync(T command, ProcessMessageEventArgs serviceBusArguments,
        CancellationToken stoppingToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Create a processor that we can use to process the messages
        processor = client.CreateProcessor(messagingSettings.CommandsTopic, SubscriptionName,
            new ServiceBusProcessorOptions());
        resubmitter = client.CreateSender(messagingSettings.CommandsTopic);
        try
        {
            // Add handler to process messages
            processor.ProcessMessageAsync += MessageHandler;

            // add handler to process any errors
            processor.ProcessErrorAsync += ErrorHandler;

            // start processing 
            await processor.StartProcessingAsync(stoppingToken);
            while (stoppingToken.IsCancellationRequested == false)
            {
                await Task.Delay(5000, stoppingToken);
                // Check circuit breaker status or other conditions here if needed
            }
        }
        finally
        {
            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            await processor.DisposeAsync();
            await client.DisposeAsync();
        }
    }


// handle received messages
    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        try
        {
            var command = AeroCommand.FromMessage<T>(args.Message);
            //var command = JsonConvert.DeserializeObject<T>(args.Message.Body.ToString());
            using (logger.BeginScope(command.GetTraceContext()))
            {
                logger.LogInformation("Received Command: {CommandName}", SubscriptionName);
                metricService.LogCustomCounterMetric(string.Format(Metrics.CommandReceived, SubscriptionName.ToLower()),
                    1, command.GetTraceContext());
                var actions = await ProcessCommandAsync(command, args, args.CancellationToken);
                switch (actions.Action)
                {
                    case MessageAction.Complete:
                        // complete the message. message is deleted from the queue. 
                        await args.CompleteMessageAsync(args.Message);
                        metricService.LogCustomCounterMetric(
                            string.Format(Metrics.CommandCompleted, SubscriptionName.ToLower()), 1,
                            command.GetTraceContext());
                        break;
                    case MessageAction.Abandon:
                        // complete the message. message is deleted from the queue. 
                        await args.AbandonMessageAsync(args.Message);
                        metricService.LogCustomCounterMetric(
                            string.Format(Metrics.CommandAbandoned, SubscriptionName.ToLower()), 1,
                            command.GetTraceContext());
                        break;
                    case MessageAction.Deadletter:
                        // dead letter message
                        if (actions is DeadletterMessageOption dla)
                        {
                            await args.DeadLetterMessageAsync(args.Message, dla.Reason);
                            metricService.LogCustomCounterMetric(
                                string.Format(Metrics.CommandDeadlettered, SubscriptionName.ToLower()), 1,
                                command.GetTraceContext());
                        }

                        break;
                    case MessageAction.Defer:
                        // dead letter message
                        if (actions is DeferMessageOption dfa)
                        {
                            // TODO : defer logic !!
                            var enqueueTime = DateTimeOffset.UtcNow.Add(dfa.Timespan);

                            logger?.LogWarning(
                                "Will postpone message {MessageId} with command type {CommandType} to {EnqueueTime}",
                                args.Message?.MessageId,
                                args.Message?.ApplicationProperties?["CommandType"] ?? "NotSet", enqueueTime);
                            var resubmitMessage = new ServiceBusMessage(args.Message);

                            resubmitMessage.ScheduledEnqueueTime = enqueueTime;
                            resubmitMessage.MessageId = args.Message.MessageId + "_R";
                            resubmitMessage.ApplicationProperties["Retry"] = true;
                            await resubmitter.SendMessageAsync(resubmitMessage);
                            await args.CompleteMessageAsync(args.Message);

                            metricService.LogCustomCounterMetric(
                                string.Format(Metrics.CommandDefered, SubscriptionName.ToLower()), 1,
                                command.GetTraceContext());
                        }

                        break;
                    case MessageAction.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        catch (Exception e)
        {
            await args.DeadLetterMessageAsync(args.Message, e.Message);
            metricService.LogCustomCounterMetric(string.Format(Metrics.CommandDeadlettered, SubscriptionName.ToLower()),
                1);
            logger.LogError(e, "An unhandled exception occurred: {Message}", e.Message);
        }
    }

    // handle any errors when receiving messages
    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        logger.LogError(args.Exception, "An unhandled exception occurred: {Message}", args.Exception.Message);
        return Task.CompletedTask;
    }
}