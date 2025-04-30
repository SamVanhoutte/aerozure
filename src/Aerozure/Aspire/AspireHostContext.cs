namespace Aerozure.Aspire;

public class AspireHostContext
{
    public static bool IsRunningInAspireAppHost =>
        !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPIRE_RESOURCE_SERVICE_ENDPOINT_URL"))
        || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CONTAINER_APP_HOSTNAME"));
}