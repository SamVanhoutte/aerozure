namespace Aerozure.Communication.Mailing;

public class Email
{
    public Email(string recipientAddress, string recipientName, object mergeContent)
    {
        RecipientName = recipientName;
        RecipientAddress = recipientAddress;
        MergeContent = mergeContent;
    }
    public string RecipientName { get; set; }
    public string RecipientAddress { get; set; }
    public object MergeContent { get; set; }
}