using System.Text.Json.Serialization;

namespace CyclingStats.Logic.Prediction.Azureml;

public class InputData
{
    [JsonPropertyName("columns")]
    public string[] Columns { get; set; }
    [JsonPropertyName("index")]
    public int[] Index { get; set; }
    [JsonPropertyName("data")]
    public object[][] Data { get; set; }
}