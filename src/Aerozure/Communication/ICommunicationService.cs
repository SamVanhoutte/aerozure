using Aerozure.Communication.Context;

namespace Aerozure.Communication;

public interface ICommunicationService
{
    Task<CommunicationContext> SendAsync(CommunicationContext context, Func<CommunicationContext, Task>? loggingTask );
}