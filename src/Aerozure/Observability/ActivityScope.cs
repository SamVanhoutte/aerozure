using System.Diagnostics;
using Aerozure.Configuration;

namespace Aerozure.Observability;

public interface IActivityObserver
{
    
}
public class ActivityObserver : IActivityObserver
{
    private readonly ActivitySource? activitySource;

    public ActivityObserver()
    {
        if (!string.IsNullOrEmpty(ObservabilityOptions.TraceActivitySource))
        {
            activitySource = new ActivitySource(ObservabilityOptions.TraceActivitySource);
        }
    }
    public async Task ExecuteAsync(string eventName, Func<Task> action)
    {
        if (activitySource == null)
        {
            await action();
        }
        // using var activity = activitySource.StartActivity("UserLogin");
        // activity?.SetTag("user.id", userId);
        //
        // _logger.LogInformation("User {UserId} is attempting to log in", userId);
        // CustomRequestCounter.Add(1, new KeyValuePair<string, object?>("operation", "login"));
        //
        // activity?.AddEvent(new ActivityEvent("UserLoggedIn", tags: new ActivityTagsCollection
        // {
        //     { "userId", userId },
        //     { "timestamp", DateTime.UtcNow }
        // }));
        // await action();
    }
}