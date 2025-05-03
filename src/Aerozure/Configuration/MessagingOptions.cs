namespace Aerozure.Configuration;

public class MessagingOptions(MessagingType messagingType)
{
    public MessagingType MessagingType => messagingType;
}