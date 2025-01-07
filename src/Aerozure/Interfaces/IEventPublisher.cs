using Aerozure.Commands;

namespace Aerozure.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishEventAsync(AeroEvent @event, TimeSpan? delay = null);
        Task PublishEventsAsync(IEnumerable<AeroEvent> events, TimeSpan? maxDelay = null);
    }
}