namespace Aerozure.Configuration;

public class LoggingConfigurationOptions
{
    private bool enableApplicationInsights = false;
    public bool EnableApplicationInsights => enableApplicationInsights;
    public string? ServiceName { private set; get; }
    public string? ApplicationInsightsConnectionString { private set; get; }
    
    public void ConfigureApplicationInsights( string? connectionString = null, string? serviceName = null)
    {
        enableApplicationInsights = true;
        ServiceName = serviceName ?? "aerozure";
        ApplicationInsightsConnectionString = connectionString;
    }
}