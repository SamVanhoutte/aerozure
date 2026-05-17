namespace Aerozure.Communication.Context;

public class CommunicationContext
{
    public string? Subject { get; set; }
    public object? StructuralData { get; set; }
    public EmailContext Email { get; set; } = new();
    public SmsContext Sms { get; set; } = new();
    public WhatsAppContext WhatsApp { get; set; } = new();
    public Guid? OrganizationId { get; set; }
    public string NotificationType { get; set; }
    public string? Hyperlink { get; set; }

    public string MessageId { get; set; }
    public string Body { get; set; }
}