using Aerozure.Configuration;
using Aerozure.Interfaces;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

namespace Aerozure.Commands
{
    public class ServiceBusTransmitter : ICommandTransmitter, IEventPublisher
    {
        private readonly ServiceBusClient serviceBusClient;
        private readonly IMessageGenerator<ServiceBusMessage> messageGenerator;
        private readonly ServiceBusSettings settings;

        public ServiceBusTransmitter(
            ServiceBusClient serviceBusClient,
            IMessageGenerator<ServiceBusMessage> messageGenerator,
            IOptions<ServiceBusSettings> settings)
        {
            this.settings = settings?.Value;
            this.serviceBusClient = serviceBusClient;
            this.messageGenerator = messageGenerator;
        }


        public async Task SendCommandAsync(AeroCommand command, TimeSpan? delay = null, IDictionary<string, object>? additionalProperties = null)
        {
            var sender = serviceBusClient.CreateSender(settings.CommandTopicName);
            var message = messageGenerator.CreateMessage(command, delay, additionalProperties);

            await sender.SendMessageAsync(message);
        }

        public async Task SendCommandsAsync(IEnumerable<AeroCommand> commands, TimeSpan? maxDelay = null, IDictionary<string, object>? additionalProperties = null)
        {
            var sender = serviceBusClient.CreateSender(settings.CommandTopicName);
            var maxSeconds = (int)Math.Round( maxDelay?.TotalSeconds ?? 0);
            var rnd = new Random();
            await sender.SendMessagesAsync(commands.Select(cmd =>
                messageGenerator.CreateMessage(cmd, TimeSpan.FromSeconds(rnd.Next(0, maxSeconds)), additionalProperties)));
        }

        public async Task PublishEventAsync(AeroEvent @event, TimeSpan? delay = null)
        {
            var sender = serviceBusClient.CreateSender(settings.EventTopicName);
            var message = messageGenerator.CreateMessage(@event, delay);

            await sender.SendMessageAsync(message);
        }

        public async Task PublishEventsAsync(IEnumerable<AeroEvent> events, TimeSpan? maxDelay = null)
        {
            var sender = serviceBusClient.CreateSender(settings.EventTopicName);
            var maxSeconds = (int)Math.Round( maxDelay?.TotalSeconds ?? 0);
            var rnd = new Random();
            await sender.SendMessagesAsync(events.Select(cmd =>
                messageGenerator.CreateMessage(cmd, TimeSpan.FromSeconds(rnd.Next(0, maxSeconds)))));
        }
    }
}