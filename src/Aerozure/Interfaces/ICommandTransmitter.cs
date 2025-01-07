using Aerozure.Commands;

namespace Aerozure.Interfaces
{
    public interface ICommandTransmitter
    {
        Task SendCommandAsync(AeroCommand command, TimeSpan? delay = null);
        Task SendCommandsAsync(IEnumerable<AeroCommand> commands, TimeSpan? maxDelay = null);
    }
}