namespace Aerozure.Communication.Mailing;

public class Email(string recipientAddress, string recipientName, object mergeContent)
{
    public string RecipientName { get; set; } = recipientName;
    public string RecipientAddress { get; set; } = recipientAddress;
    public object MergeContent { get; set; } = mergeContent;
}