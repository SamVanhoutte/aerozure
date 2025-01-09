namespace Aerozure.Communication.Context;

public class CommunicationContext
{

    public CommunicationContext()
    {
        Email = new EmailContext();
        Sms = new SmsContext();
        WhatsApp = new WhatsAppContext();
    }
    public string? Subject { get; set; }
    public object? StructuralData { get; set; }
    public EmailContext Email { get; set; }
    public SmsContext Sms { get; set; }
    public WhatsAppContext WhatsApp { get; set; }
    public Guid? OrganizationId { get; set; }
    public string NotificationType { get; set; }
    public string? Hyperlink { get; set; }

    public string MessageId { get; set; }
    public string Body { get; set; }
}