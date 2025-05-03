namespace Aerozure.Configuration;

public class AeroStartupOptions
{
    public static AeroStartupOptions Default => new AeroStartupOptions();
    
    internal bool EnableMessaging = false;
    internal bool EnableCommunication = false;
    internal bool EnableAspire => AspireOptions != null;
    public AspireHostingOptions? AspireOptions { get; set; }
    public MessagingOptions? MessagingOptions { get; set; }
    public void ConfigureCommunication(CommunicationOptions? options)
    {
        EnableCommunication = options != null;
    }
    
    public void ConfigureMessaging(MessagingOptions? options)
    {
        EnableMessaging = options != null;
        MessagingOptions = options; 
    }
    
    public void ConfigureAspire(AspireHostingOptions? options)
    {
        AspireOptions = options;
    }
}