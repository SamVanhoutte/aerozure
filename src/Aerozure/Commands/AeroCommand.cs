using Aerozure.Tracing;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aerozure.Commands
{
    public abstract class AeroCommand 
    {
        public AeroCommand(string commandType)
        {
            this.CommandType = commandType.ToString();
        }

        [TraceProperty]
        public string CommandType { get; }

        public string ToJson()
        {
            var jo = JObject.FromObject(this);
            return jo.ToString();
        }
        
        public static T FromMessage<T>(ServiceBusReceivedMessage message) where T: AeroCommand
        {
            var bodyContent = message.Body.ToString();
            var command = JsonConvert.DeserializeObject<T>(bodyContent);
            return command;
        }

        public virtual string GenerateSessionId()
        {
            return Guid.NewGuid().ToString();
        }

        public abstract string GenerateMessageId();
        
        public static string GetSubscriptionName<T>()
        {
            if (typeof(T).IsGenericType)
            {
                var genericType = typeof(T).GetGenericTypeDefinition();
                var genericArgs = typeof(T).GetGenericArguments();
                return $"{genericType.Name}-{string.Join("-", genericArgs.Select(arg => arg.Name))}".Replace("`1", string.Empty);
            }
            return typeof(T).Name;
        }
    }
}