namespace Aerozure.Communication.Context;

public class EmailContext
{
    public EmailAddress Recipient { get; set; }
    public CommResult Result { get; set; }
    public string? Body { get; set; }
    public string? TemplateId { get; set; }
}