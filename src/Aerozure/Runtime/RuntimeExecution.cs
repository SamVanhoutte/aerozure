using Azure.Identity;

namespace Aerozure.Runtime
{
    public class RuntimeExecution
    {
        public static bool IsLocal => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));

        public static DefaultAzureCredential Credentials =>
            new(new DefaultAzureCredentialOptions
            {
                Retry = { NetworkTimeout = TimeSpan.FromSeconds(10)}
            });
    }
}