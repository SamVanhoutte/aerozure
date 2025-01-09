namespace Aerozure.Communication.Context;

public class WhatsAppContext
{
    public string? PhoneNumber { get; set; }
    public string TransactionId { get; set; }
    public CommResult Result { get; set; }

    public string? Body { get; set; }
    public string? Template { get; set; }
}