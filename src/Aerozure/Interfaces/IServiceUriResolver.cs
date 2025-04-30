namespace Aerozure.Interfaces;

public interface IServiceUriResolver
{
    Task<Uri> GetServiceUriAsync(string serviceName);
}