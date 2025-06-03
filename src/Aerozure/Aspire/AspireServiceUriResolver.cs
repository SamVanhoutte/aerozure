using Aerozure.Configuration;
using Aerozure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.ServiceDiscovery;

namespace Aerozure.Aspire;

public class AspireServiceUriResolver(IConfiguration configuration, IOptions<AspireHostingOptions> aspireOptions)
    : IServiceUriResolver
{
    public Uri GetServiceUri(string serviceName)
    {
        var configEntry = $"{serviceName}_url";
        if (!string.IsNullOrEmpty(aspireOptions.Value?.ServiceUrlConfigurationKey))
        {
            configEntry = string.Format(aspireOptions.Value.ServiceUrlConfigurationKey, serviceName);
        }

        var aspireHosted = AspireHostContext.IsRunningInAspireAppHost;

        return aspireHosted
            // We return the .NET Aspire url
            ? new Uri($"http://{serviceName}")
            // We return the configured fallback url
            : new Uri(configuration[configEntry] ?? throw new ArgumentNullException(configEntry));
    }

    public Task<Uri> GetServiceUriAsync(string serviceName)
    {
        return Task.FromResult(GetServiceUri(serviceName));   
    }
}