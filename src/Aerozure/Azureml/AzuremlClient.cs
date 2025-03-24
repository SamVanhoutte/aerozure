using System.Net.Http.Json;
using Aerozure.Configuration;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace Aerozure.Azureml;

public class AzuremlClient
{
    private readonly AzuremlOptions mlOptions;
    
    public AzuremlClient(IOptions<AzuremlOptions> mlOptions)
    {
        this.mlOptions = mlOptions.Value;
    }
    public async Task<T?> CallInferenceAsync<T>(object request)
    {
        var mlRequest = AzuremlRequest.Create(request);
        var response = await mlOptions.InferenceEndpoint
            .WithHeader("Authorization", $"Bearer {mlOptions.BearerToken}")
            .PostJsonAsync(mlRequest);
        return await response.ResponseMessage.Content.ReadFromJsonAsync<T>();
    }
}