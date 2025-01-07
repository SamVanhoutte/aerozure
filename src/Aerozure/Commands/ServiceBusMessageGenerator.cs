using Aerozure.Interfaces;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Aerozure.Commands
{
    public class ServiceBusMessageGenerator : IMessageGenerator<ServiceBusMessage>
    {
        public ServiceBusMessage CreateMessage(AeroCommand command, TimeSpan? commandDelay = null)
        {
            var message = new ServiceBusMessage(command.ToJson());
            message.ApplicationProperties.Add(ServiceBusConstants.CommandTypePropertyName, command.CommandType);

            message.SessionId = command.GenerateSessionId();
            message.MessageId = command.GenerateMessageId();

            if (commandDelay.HasValue)
            {
                message.ScheduledEnqueueTime = DateTimeOffset.UtcNow.Add(commandDelay.Value);
            }

            return message;
        }

        public ServiceBusMessage CreateMessage(AeroEvent @event, TimeSpan? messageDelay = null)
        {
            var message = new ServiceBusMessage(@event.ToJson());
            message.ApplicationProperties.Add(ServiceBusConstants.EventTypePropertyName, @event.EventType);

            message.MessageId = @event.MessageId;

            if (messageDelay.HasValue)
            {
                message.ScheduledEnqueueTime = DateTimeOffset.UtcNow.Add(messageDelay.Value);
            }

            return message;
        }

        public T ParseCommand<T>(string messageBody, IDictionary<string, string> metadata = null) where T : AeroCommand
        {
            var command = JsonConvert.DeserializeObject<T>(messageBody);
            return command;
        }

        public T ParseEvent<T>(string messageBody, IDictionary<string, string> metadata = null) where T : AeroEvent
        {
            var command = JsonConvert.DeserializeObject<T>(messageBody);
            return command;
        }

        public T ParseCommand<T>(ServiceBusMessage message) where T : AeroCommand
        {
            var bodyContent = message.Body.ToString();
            var command = JsonConvert.DeserializeObject<T>(bodyContent);
            return command;
        }
    }
}