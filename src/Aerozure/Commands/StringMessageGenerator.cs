using Aerozure.Interfaces;
using Azure.Messaging.ServiceBus;
using Flurl.Util;
using Newtonsoft.Json;

namespace Aerozure.Commands
{
    public class StringMessageGenerator : IMessageGenerator<string>
    {
        public string CreateMessage(AeroCommand command, TimeSpan? commandDelay = null, IDictionary<string, object>? additionalProperties = null)
        {
            return command.ToJson();
        }

        public string CreateMessage(AeroEvent @event, TimeSpan? messageDelay = null, IDictionary<string, object>? additionalProperties = null)
        {
            return @event.ToJson();
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