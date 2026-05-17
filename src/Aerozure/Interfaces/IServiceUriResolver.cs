namespace Aerozure.Interfaces;

public interface IServiceUriResolver
{
    Uri GetServiceUri(string serviceName);
    Task<Uri> GetServiceUriAsync(string serviceName);
}