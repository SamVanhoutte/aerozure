using System.Net.Http.Json;
using System.Reflection;
using Aerozure.Azureml;
using Aerozure.Configuration;
using CyclingStats.Logic.Prediction.Azureml;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace CyclingStats.Logic.Prediction;

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