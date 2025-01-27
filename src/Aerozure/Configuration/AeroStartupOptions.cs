namespace Aerozure.Configuration;

public class AeroStartupOptions
{
    public static AeroStartupOptions Default => new AeroStartupOptions();
    
    internal bool EnableMessaging = false;
    internal bool EnableCommunication = false;

    public void ConfigureCommunication(CommunicationOptions? options)
    {
        EnableCommunication = options != null;
    }
    
    public void ConfigureMessaging(MessagingOptions? options)
    {
        EnableMessaging = options != null;
    }
}