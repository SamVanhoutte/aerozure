namespace Aerozure.Communication.Context;

public class SmsContext
{
    public string? PhoneNumber { get; set; }
    public string? DisplayName { get; set; }
    public string? TransactionId { get; set; }
    public CommResult Result { get; set; }

    public string? Body { get; set; }
}