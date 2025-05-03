using Aerozure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aerozure.Commands;

public class DebugEventTransmitter : ICommandTransmitter, IEventPublisher
{
    private readonly IMessageGenerator<string> messageGenerator;
    private readonly ILogger<DebugEventTransmitter> logger;

    public DebugEventTransmitter(IMessageGenerator<string> messageGenerator, ILogger<DebugEventTransmitter> logger)
    {
        this.messageGenerator = messageGenerator;
        this.logger = logger;
    }


    public async Task SendCommandAsync(AeroCommand command, TimeSpan? delay = null, IDictionary<string, object>? additionalProperties = null)
    {
        var message = messageGenerator.CreateMessage(command, delay, additionalProperties);
        logger.LogInformation(message);
    }

    public async Task SendCommandsAsync(IEnumerable<AeroCommand> commands, TimeSpan? maxDelay = null, IDictionary<string, object>? additionalProperties = null)
    {
        foreach (var cmd in commands)
        {
            var message = messageGenerator.CreateMessage(cmd, maxDelay, additionalProperties);
            logger.LogInformation(message);
        }
    }

    public async Task PublishEventAsync(AeroEvent @event, TimeSpan? delay = null)
    {
        var message = messageGenerator.CreateMessage(@event, delay);
        logger.LogInformation(message);
    }

    public async Task PublishEventsAsync(IEnumerable<AeroEvent> events, TimeSpan? maxDelay = null)
    {
        foreach (var ev in events)
        {
            var message = messageGenerator.CreateMessage(ev, maxDelay);
            logger.LogInformation(message);
        }
    }
}