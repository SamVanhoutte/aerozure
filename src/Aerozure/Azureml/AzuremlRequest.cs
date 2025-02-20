using System.Reflection;
using System.Text.Json.Serialization;
using CyclingStats.Logic.Prediction.Azureml;

namespace Aerozure.Azureml;

public class AzuremlRequest
{
    [JsonPropertyName("input_data")] public InputData InputData { get; set; }

    public static AzuremlRequest Create(object payload)
    {
        var columns = new List<string>();
        var data = new List<object>();
        foreach (PropertyInfo property in payload.GetType().GetProperties())
        {
            columns.Add(property.Name);
            data.Add(property.GetValue(payload, null));
        }

        var dataArray = new object[] { data.ToArray() };
        return new AzuremlRequest
        {
            InputData = new InputData
            {
                Columns = columns.ToArray(),
                Index = new int[] { 1 },
                Data = (new List<object[]> { dataArray }).ToArray()
            }
        };
    }
}