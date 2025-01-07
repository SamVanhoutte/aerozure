namespace Aerozure.Configuration
{
    public class ServiceBusSettings
    {
        public string FullyQualifiedNamespace { get; set; } = null!;

        public string CommandTopicName { get; set; } = null!;
        public string EventTopicName { get; set; } = null!;
    }
}