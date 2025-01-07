using Aerozure.Commands;

namespace Aerozure.Interfaces
{
    public interface IMessageGenerator<TMessageType>
    {
        public TMessageType CreateMessage(AeroCommand command, TimeSpan? commandDelay = null);
        public TMessageType CreateMessage(AeroEvent @event, TimeSpan? messageDelay = null);
        public T ParseCommand<T>(string messageBody, IDictionary<string, string> metadata = null) where T : AeroCommand;
        public T ParseEvent<T>(string messageBody, IDictionary<string, string> metadata = null) where T : AeroEvent;
    }
}