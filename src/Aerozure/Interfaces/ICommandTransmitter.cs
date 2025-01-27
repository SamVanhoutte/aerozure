using Aerozure.Commands;
using Azure.Messaging.ServiceBus;

namespace Aerozure.Interfaces
{
    public interface ICommandTransmitter
    {
        Task SendCommandAsync(AeroCommand command, TimeSpan? delay = null, IDictionary<string, object>? additionalProperties = null);
        Task SendCommandsAsync(IEnumerable<AeroCommand> commands, TimeSpan? maxDelay = null, IDictionary<string, object>? additionalProperties = null);
    }
}