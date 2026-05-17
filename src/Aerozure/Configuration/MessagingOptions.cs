namespace Aerozure.Configuration;

public class MessagingOptions
{
    public  MessagingType MessagingType { get; set; }

    public MessagingOptions()
    {
        MessagingType = MessagingType.AzureServiceBus;
    }

    public MessagingOptions(MessagingType messagingType)
    {
        this.MessagingType = messagingType;
    }
    public string? CommandsTopic { get; set; } = "Commands";
}