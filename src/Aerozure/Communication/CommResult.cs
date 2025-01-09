using SendGrid;

namespace Aerozure.Communication;

public class CommResult
{
    public bool Success { get; set; }
    public string TransactionId { get; set; }
    public string ErrorMessage { get; set; }
    public string StatusCode { get; set; }

    public static CommResult FromResponse(Response response)
    {
        return new CommResult
        {
            Success = response.IsSuccessStatusCode,
            TransactionId = response.Headers.GetValues("X-Message-Id").FirstOrDefault(),
            StatusCode = response.StatusCode.ToString(),
            ErrorMessage = response.StatusCode == System.Net.HttpStatusCode.OK
                ? string.Empty
                : response.Body.ReadAsStringAsync().Result
        };
    }

    public static CommResult NotSent =>
        new CommResult
        {
            Success = false,
            TransactionId = string.Empty,
            StatusCode = "",
            ErrorMessage = "Transmission not attempted"
        };
    
    public static CommResult Expired =>
        new CommResult
        {
            Success = false,
            TransactionId = string.Empty,
            StatusCode = "",
            ErrorMessage = "Transmission expired"
        };
}